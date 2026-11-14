using BhaavBridge.Web.Data;
using BhaavBridge.Web.Models.Entities;
using BhaavBridge.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BhaavBridge.Web.Controllers;

[Authorize]
public sealed class OrderController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public OrderController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [Authorize(Roles = "Buyer")]
    [HttpGet]
    public async Task<IActionResult> BuyerOrders()
    {
        var buyer = await GetBuyerAsync();
        if (buyer == null) return Forbid();

        var orders = await BuildOrderQuery()
            .Where(o => o.Offer.BuyerProfileId == buyer.BuyerProfileId)
            .OrderByDescending(o => o.CreatedAtUtc)
            .Select(ToSummary())
            .ToListAsync();

        return View("BuyerOrders", orders);
    }

    [Authorize(Roles = "Farmer")]
    [HttpGet]
    public async Task<IActionResult> FarmerOrders()
    {
        var farmer = await GetFarmerAsync();
        if (farmer == null) return Forbid();

        var orders = await BuildOrderQuery()
            .Where(o => o.Offer.ProduceLot.FarmerProfileId == farmer.FarmerProfileId)
            .OrderByDescending(o => o.CreatedAtUtc)
            .Select(ToSummary())
            .ToListAsync();

        return View("FarmerOrders", orders);
    }

    [Authorize(Roles = "Administrator")]
    [HttpGet]
    public async Task<IActionResult> All()
    {
        var orders = await BuildOrderQuery()
            .OrderByDescending(o => o.CreatedAtUtc)
            .Select(ToSummary())
            .ToListAsync();

        return View("BuyerOrders", orders);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var order = await BuildOrderQuery()
            .Where(o => o.OrderId == id)
            .Select(ToSummary())
            .FirstOrDefaultAsync();

        if (order == null) return NotFound();

        if (!await CanViewOrderAsync(id))
            return Forbid();

        return View(order);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(int id, OrderStatus status)
    {
        var order = await _context.Orders
            .Include(o => o.Offer)
                .ThenInclude(o => o.ProduceLot)
            .Include(o => o.Offer)
                .ThenInclude(o => o.BuyerProfile)
            .FirstOrDefaultAsync(o => o.OrderId == id);

        if (order == null) return NotFound();

        var userId = _userManager.GetUserId(User);
        var isFarmerOwner = User.IsInRole("Farmer") &&
            await _context.FarmerProfiles.AnyAsync(p =>
                p.UserId == userId &&
                p.FarmerProfileId == order.Offer.ProduceLot.FarmerProfileId);
        var isBuyerOwner = User.IsInRole("Buyer") &&
            await _context.BuyerProfiles.AnyAsync(p =>
                p.UserId == userId &&
                p.BuyerProfileId == order.Offer.BuyerProfileId);

        var allowed = false;

        if (isFarmerOwner)
        {
            allowed =
                (order.Status == OrderStatus.Placed && status == OrderStatus.Confirmed) ||
                (order.Status == OrderStatus.Confirmed && status == OrderStatus.ReadyForPickup);
        }
        else if (isBuyerOwner)
        {
            allowed = order.Status == OrderStatus.ReadyForPickup &&
                      status == OrderStatus.Completed;
        }
        else if (User.IsInRole("Administrator"))
        {
            allowed = status == OrderStatus.Cancelled &&
                      order.Status != OrderStatus.Completed &&
                      order.Status != OrderStatus.Cancelled;
        }

        if (!allowed)
            return Forbid();

        order.Status = status;
        order.UpdatedAtUtc = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Order status updated.";
        return RedirectToAction(nameof(Details), new { id });
    }

    private IQueryable<Order> BuildOrderQuery()
    {
        return _context.Orders
            .AsNoTracking()
            .Include(o => o.Offer)
                .ThenInclude(o => o.ProduceLot)
                    .ThenInclude(l => l.Crop)
            .Include(o => o.Offer)
                .ThenInclude(o => o.ProduceLot)
                    .ThenInclude(l => l.FarmerProfile)
                        .ThenInclude(f => f.User)
            .Include(o => o.Offer)
                .ThenInclude(o => o.BuyerProfile);
    }

    private static System.Linq.Expressions.Expression<Func<Order, OrderSummaryViewModel>> ToSummary()
    {
        return o => new OrderSummaryViewModel
        {
            OrderId = o.OrderId,
            OfferId = o.OfferId,
            ProduceLotId = o.Offer.ProduceLotId,
            CropName = o.Offer.ProduceLot.Crop.Name,
            FarmerName = o.Offer.ProduceLot.FarmerProfile.User.FullName,
            BuyerBusinessName = o.Offer.BuyerProfile.BusinessName,
            QuantityQuintals = o.AgreedQuantityQuintals,
            AgreedPricePerQuintal = o.AgreedPricePerQuintal,
            GrossValue = o.GrossValue,
            TransportationCost = o.TransportationCostSnapshot,
            LoadingCost = o.LoadingCostSnapshot,
            OtherFarmerCost = o.OtherFarmerCostSnapshot,
            TotalFarmerCosts = o.TotalFarmerCostsSnapshot,
            EstimatedNetReturn = o.EstimatedNetReturnSnapshot,
            Status = o.Status,
            CreatedAtUtc = o.CreatedAtUtc,
            UpdatedAtUtc = o.UpdatedAtUtc
        };
    }

    private async Task<BuyerProfile?> GetBuyerAsync()
    {
        var userId = _userManager.GetUserId(User);
        return userId == null ? null :
            await _context.BuyerProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
    }

    private async Task<FarmerProfile?> GetFarmerAsync()
    {
        var userId = _userManager.GetUserId(User);
        return userId == null ? null :
            await _context.FarmerProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
    }

    private async Task<bool> CanViewOrderAsync(int orderId)
    {
        if (User.IsInRole("Administrator")) return true;

        var userId = _userManager.GetUserId(User);
        if (userId == null) return false;

        var query = _context.Orders.Where(o => o.OrderId == orderId);

        if (User.IsInRole("Buyer"))
            return await query.AnyAsync(o => o.Offer.BuyerProfile.UserId == userId);

        if (User.IsInRole("Farmer"))
            return await query.AnyAsync(o => o.Offer.ProduceLot.FarmerProfile.UserId == userId);

        return false;
    }
}
