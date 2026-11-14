using BhaavBridge.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BhaavBridge.Web.Controllers.Api;

[ApiController]
[Route("api/market-reference-prices")]
[Authorize(Roles = "Farmer,Buyer,Administrator")]
public sealed class MarketReferencePricesApiController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public MarketReferencePricesApiController(ApplicationDbContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] int? cropId,
        [FromQuery] string? district,
        [FromQuery] string? state)
    {
        var query = _context.MarketReferencePrices.AsNoTracking().AsQueryable();
        if (cropId.HasValue) query = query.Where(p => p.CropId == cropId.Value);
        if (!string.IsNullOrWhiteSpace(district)) query = query.Where(p => p.District.Contains(district.Trim()));
        if (!string.IsNullOrWhiteSpace(state)) query = query.Where(p => p.State.Contains(state.Trim()));

        var rows = await query.OrderByDescending(p => p.EffectiveDate)
            .Select(p => new
            {
                p.MarketReferencePriceId,
                CropName = p.Crop.Name,
                p.MarketName,
                p.District,
                p.State,
                p.EffectiveDate,
                p.MinimumPrice,
                p.MaximumPrice,
                p.ModalPrice,
                p.SourceName,
                p.SourceUrl
            }).ToListAsync();

        return Ok(rows);
    }
}
