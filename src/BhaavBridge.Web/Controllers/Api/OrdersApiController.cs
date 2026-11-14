using BhaavBridge.Web.Data;
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
}
