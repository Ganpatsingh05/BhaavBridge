using BhaavBridge.Web.Data;
using BhaavBridge.Web.Models.Api;
using BhaavBridge.Web.Models.Entities;
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

    [Authorize(Roles = "Administrator")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] MarketReferencePriceRequest request)
    {
        var validation = ValidateRequest(request);
        if (validation != null) return validation;

        var cropIsActive = await _context.Crops.AnyAsync(c => c.CropId == request.CropId && c.IsActive);
        if (!cropIsActive) return BadRequest(new { message = "Select an active crop." });

        var row = new MarketReferencePrice
        {
            CropId = request.CropId,
            MarketName = request.MarketName.Trim(),
            District = request.District.Trim(),
            State = request.State.Trim(),
            EffectiveDate = request.EffectiveDate,
            MinimumPrice = request.MinimumPrice,
            MaximumPrice = request.MaximumPrice,
            ModalPrice = request.ModalPrice,
            SourceName = request.SourceName.Trim(),
            SourceUrl = string.IsNullOrWhiteSpace(request.SourceUrl) ? null : request.SourceUrl.Trim(),
            RecordedAtUtc = DateTime.UtcNow
        };

        _context.MarketReferencePrices.Add(row);
        await _context.SaveChangesAsync();

        return Created($"/api/market-reference-prices/{row.MarketReferencePriceId}", new
        {
            row.MarketReferencePriceId,
            row.CropId,
            row.MarketName,
            row.EffectiveDate,
            row.ModalPrice,
            row.SourceName
        });
    }

    [Authorize(Roles = "Administrator")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] MarketReferencePriceRequest request)
    {
        var row = await _context.MarketReferencePrices.FindAsync(id);
        if (row == null) return NotFound();

        var validation = ValidateRequest(request);
        if (validation != null) return validation;

        if (!await _context.Crops.AnyAsync(c => c.CropId == request.CropId && c.IsActive))
            return BadRequest(new { message = "Select an active crop." });

        row.CropId = request.CropId;
        row.MarketName = request.MarketName.Trim();
        row.District = request.District.Trim();
        row.State = request.State.Trim();
        row.EffectiveDate = request.EffectiveDate;
        row.MinimumPrice = request.MinimumPrice;
        row.MaximumPrice = request.MaximumPrice;
        row.ModalPrice = request.ModalPrice;
        row.SourceName = request.SourceName.Trim();
        row.SourceUrl = string.IsNullOrWhiteSpace(request.SourceUrl) ? null : request.SourceUrl.Trim();

        await _context.SaveChangesAsync();
        return Ok(new { row.MarketReferencePriceId, row.MarketName, row.EffectiveDate, row.ModalPrice });
    }

    private IActionResult? ValidateRequest(MarketReferencePriceRequest request)
    {
        if (request.MinimumPrice.HasValue && request.MaximumPrice.HasValue &&
            request.MinimumPrice.Value > request.MaximumPrice.Value)
            return BadRequest(new { message = "Maximum price must be greater than or equal to minimum price." });

        if (request.MinimumPrice.HasValue && request.ModalPrice < request.MinimumPrice.Value)
            return BadRequest(new { message = "Modal price cannot be below minimum price." });

        if (request.MaximumPrice.HasValue && request.ModalPrice > request.MaximumPrice.Value)
            return BadRequest(new { message = "Modal price cannot exceed maximum price." });

        if (request.EffectiveDate == default)
            return BadRequest(new { message = "Effective date is required." });

        return null;
    }

}
