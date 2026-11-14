using BhaavBridge.Web.Data;
using BhaavBridge.Web.Models.Api;
using BhaavBridge.Web.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BhaavBridge.Web.Controllers.Api;

[ApiController]
[Route("api/produce-lots")]
[Authorize(Roles = "Farmer,Buyer,Administrator")]
public sealed class ProduceLotsApiController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public ProduceLotsApiController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int? cropId,
        [FromQuery] string? district,
        [FromQuery] string? state,
        [FromQuery] decimal? minPrice,
        [FromQuery] decimal? maxPrice,
        [FromQuery] decimal? minQuantity,
        [FromQuery] decimal? maxQuantity)
    {
        var query = _context.ProduceLots.AsNoTracking().AsQueryable();

        if (User.IsInRole("Farmer"))
        {
            var userId = _userManager.GetUserId(User);
            var farmer = await _context.FarmerProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
            if (farmer == null) return Forbid();
            query = query.Where(l => l.FarmerProfileId == farmer.FarmerProfileId);
        }
        else
        {
            query = query.Where(l => l.Status == ProduceLotStatus.Published);
        }

        if (cropId.HasValue) query = query.Where(l => l.CropId == cropId.Value);
        if (!string.IsNullOrWhiteSpace(district)) query = query.Where(l => l.District.Contains(district.Trim()));
        if (!string.IsNullOrWhiteSpace(state)) query = query.Where(l => l.State.Contains(state.Trim()));
        if (minPrice.HasValue) query = query.Where(l => l.AskingPricePerQuintal >= minPrice.Value);
        if (maxPrice.HasValue) query = query.Where(l => l.AskingPricePerQuintal <= maxPrice.Value);
        if (minQuantity.HasValue) query = query.Where(l => l.QuantityQuintals >= minQuantity.Value);
        if (maxQuantity.HasValue) query = query.Where(l => l.QuantityQuintals <= maxQuantity.Value);

        var rows = await query.OrderByDescending(l => l.CreatedAtUtc)
            .Select(l => new
            {
                l.ProduceLotId,
                CropName = l.Crop.Name,
                l.Variety,
                l.QualityGrade,
                l.QuantityQuintals,
                l.HarvestDate,
                l.District,
                l.State,
                l.AskingPricePerQuintal,
                l.Status
            }).ToListAsync();

        return Ok(rows);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var lot = await _context.ProduceLots
            .AsNoTracking()
            .Include(l => l.Crop)
            .FirstOrDefaultAsync(l => l.ProduceLotId == id);

        if (lot == null) return NotFound();

        if (User.IsInRole("Buyer") && lot.Status != ProduceLotStatus.Published)
            return NotFound();

        if (User.IsInRole("Farmer"))
        {
            var userId = _userManager.GetUserId(User);
            var owns = await _context.FarmerProfiles.AnyAsync(p =>
                p.UserId == userId && p.FarmerProfileId == lot.FarmerProfileId);
            if (!owns) return Forbid();
        }

        return Ok(new
        {
            lot.ProduceLotId,
            CropName = lot.Crop.Name,
            lot.Variety,
            lot.QualityGrade,
            lot.QuantityQuintals,
            lot.HarvestDate,
            lot.District,
            lot.State,
            lot.AskingPricePerQuintal,
            lot.Description,
            Status = lot.Status.ToString()
        });
    }

    [Authorize(Roles = "Farmer")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProduceLotRequest request)
    {
        var userId = _userManager.GetUserId(User);
        var farmer = await _context.FarmerProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
        if (farmer == null) return Forbid();

        if (!await _context.Crops.AnyAsync(c => c.CropId == request.CropId && c.IsActive))
            return BadRequest(new { message = "Select an active crop." });

        var lot = new ProduceLot
        {
            FarmerProfileId = farmer.FarmerProfileId,
            CropId = request.CropId,
            Variety = request.Variety?.Trim(),
            QualityGrade = request.QualityGrade?.Trim(),
            QuantityQuintals = request.QuantityQuintals,
            HarvestDate = request.HarvestDate,
            District = request.District.Trim(),
            State = request.State.Trim(),
            AskingPricePerQuintal = request.AskingPricePerQuintal,
            Description = request.Description?.Trim(),
            Status = ProduceLotStatus.Draft
        };

        _context.ProduceLots.Add(lot);
        await _context.SaveChangesAsync();

        return Created($"/api/produce-lots/{lot.ProduceLotId}", new
        {
            lot.ProduceLotId,
            Status = lot.Status.ToString()
        });
    }

    [Authorize(Roles = "Farmer")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateDraft(int id, [FromBody] CreateProduceLotRequest request)
    {
        var userId = _userManager.GetUserId(User);
        var farmer = await _context.FarmerProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
        if (farmer == null) return Forbid();

        var lot = await _context.ProduceLots.FirstOrDefaultAsync(l =>
            l.ProduceLotId == id && l.FarmerProfileId == farmer.FarmerProfileId);

        if (lot == null) return NotFound();
        if (lot.Status != ProduceLotStatus.Draft)
            return Conflict(new { message = "Only Draft lots can be edited." });

        if (!await _context.Crops.AnyAsync(c => c.CropId == request.CropId && c.IsActive))
            return BadRequest(new { message = "Select an active crop." });

        lot.CropId = request.CropId;
        lot.Variety = request.Variety?.Trim();
        lot.QualityGrade = request.QualityGrade?.Trim();
        lot.QuantityQuintals = request.QuantityQuintals;
        lot.HarvestDate = request.HarvestDate;
        lot.District = request.District.Trim();
        lot.State = request.State.Trim();
        lot.AskingPricePerQuintal = request.AskingPricePerQuintal;
        lot.Description = request.Description?.Trim();

        await _context.SaveChangesAsync();
        return Ok(new { lot.ProduceLotId, Status = lot.Status.ToString() });
    }

    [Authorize(Roles = "Farmer")]
    [HttpPost("{id:int}/publish")]
    public async Task<IActionResult> Publish(int id)
    {
        var userId = _userManager.GetUserId(User);
        var farmer = await _context.FarmerProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
        if (farmer == null) return Forbid();

        var lot = await _context.ProduceLots.FirstOrDefaultAsync(l =>
            l.ProduceLotId == id && l.FarmerProfileId == farmer.FarmerProfileId);
        if (lot == null) return NotFound();
        if (lot.Status != ProduceLotStatus.Draft)
            return Conflict(new { message = "Only Draft lots can be published." });

        lot.Status = ProduceLotStatus.Published;
        await _context.SaveChangesAsync();
        return Ok(new { lot.ProduceLotId, Status = lot.Status.ToString() });
    }

    [Authorize(Roles = "Farmer")]
    [HttpPost("{id:int}/close")]
    public async Task<IActionResult> Close(int id)
    {
        var userId = _userManager.GetUserId(User);
        var farmer = await _context.FarmerProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
        if (farmer == null) return Forbid();

        var lot = await _context.ProduceLots.FirstOrDefaultAsync(l =>
            l.ProduceLotId == id && l.FarmerProfileId == farmer.FarmerProfileId);
        if (lot == null) return NotFound();

        lot.Status = ProduceLotStatus.Closed;
        await _context.SaveChangesAsync();
        return Ok(new { lot.ProduceLotId, Status = lot.Status.ToString() });
    }
}
