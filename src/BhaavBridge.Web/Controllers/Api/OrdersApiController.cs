using BhaavBridge.Web.Data;
using BhaavBridge.Web.Models.Api;
using BhaavBridge.Web.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BhaavBridge.Web.Controllers.Api;

[ApiController]
[Route("api/orders")]
[Authorize(Roles = "Buyer,Farmer,Administrator")]
public sealed class OrdersApiController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public OrdersApiController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetMine()
    {
        var userId = _userManager.GetUserId(User);
        var query = _context.Orders.AsNoTracking().AsQueryable();

        if (User.IsInRole("Buyer"))
            query = query.Where(o => o.Offer.BuyerProfile.UserId == userId);
        else if (User.IsInRole("Farmer"))
            query = query.Where(o => o.Offer.ProduceLot.FarmerProfile.UserId == userId);

        var rows = await query.OrderByDescending(o => o.CreatedAtUtc)
            .Select(o => new
            {
                o.OrderId,
                o.OfferId,
                CropName = o.Offer.ProduceLot.Crop.Name,
                FarmerName = o.Offer.ProduceLot.FarmerProfile.User.FullName,
                BuyerBusiness = o.Offer.BuyerProfile.BusinessName,
                o.AgreedQuantityQuintals,
                o.AgreedPricePerQuintal,
                o.GrossValue,
                o.TotalFarmerCostsSnapshot,
                o.EstimatedNetReturnSnapshot,
                Status = o.Status.ToString(),
                o.CreatedAtUtc
            }).ToListAsync();

        return Ok(rows);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var userId = _userManager.GetUserId(User);
        var query = _context.Orders.AsNoTracking().Where(o => o.OrderId == id);

        if (User.IsInRole("Buyer"))
            query = query.Where(o => o.Offer.BuyerProfile.UserId == userId);
        else if (User.IsInRole("Farmer"))
            query = query.Where(o => o.Offer.ProduceLot.FarmerProfile.UserId == userId);

        var row = await query.Select(o => new
        {
            o.OrderId,
            o.OfferId,
            CropName = o.Offer.ProduceLot.Crop.Name,
            o.AgreedQuantityQuintals,
            o.AgreedPricePerQuintal,
            o.GrossValue,
            o.TransportationCostSnapshot,
            o.LoadingCostSnapshot,
            o.OtherFarmerCostSnapshot,
            o.TotalFarmerCostsSnapshot,
            o.EstimatedNetReturnSnapshot,
            Status = o.Status.ToString(),
            o.CreatedAtUtc,
            o.UpdatedAtUtc
        }).FirstOrDefaultAsync();

        return row == null ? NotFound() : Ok(row);
    }

    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateOrderStatusRequest request)
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
            await _context.FarmerProfiles.AnyAsync(p => p.UserId == userId &&
                p.FarmerProfileId == order.Offer.ProduceLot.FarmerProfileId);
        var isBuyerOwner = User.IsInRole("Buyer") &&
            await _context.BuyerProfiles.AnyAsync(p => p.UserId == userId &&
                p.BuyerProfileId == order.Offer.BuyerProfileId);

        var allowed =
            (isFarmerOwner && order.Status == OrderStatus.Placed && request.Status == OrderStatus.Confirmed) ||
            (isFarmerOwner && order.Status == OrderStatus.Confirmed && request.Status == OrderStatus.ReadyForPickup) ||
            (isBuyerOwner && order.Status == OrderStatus.ReadyForPickup && request.Status == OrderStatus.Completed) ||
            (User.IsInRole("Administrator") && request.Status == OrderStatus.Cancelled &&
                order.Status != OrderStatus.Completed && order.Status != OrderStatus.Cancelled);

        if (!allowed) return Forbid();

        order.Status = request.Status;
        order.UpdatedAtUtc = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return Ok(new { order.OrderId, Status = order.Status.ToString(), order.UpdatedAtUtc });
    }


}
