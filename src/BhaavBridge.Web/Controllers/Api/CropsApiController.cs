using BhaavBridge.Web.Data;
using BhaavBridge.Web.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BhaavBridge.Web.Controllers.Api;

[ApiController]
[Route("api/crops")]
[Authorize(Roles = "Farmer,Buyer,Administrator")]
public sealed class CropsApiController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CropsApiController(ApplicationDbContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var crops = await _context.Crops
            .AsNoTracking()
            .Where(c => c.IsActive)
            .OrderBy(c => c.Name)
            .Select(c => new { c.CropId, c.Name, c.Category })
            .ToListAsync();

        return Ok(crops);
    }

    [Authorize(Roles = "Administrator")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CropRequest request)
    {
        var normalized = request.Name.Trim().ToLower();
        var exists = await _context.Crops.AnyAsync(c => c.Name.ToLower() == normalized);
        if (exists)
            return Conflict(new { message = "A crop with this name already exists." });

        var crop = new Crop
        {
            Name = request.Name.Trim(),
            Category = string.IsNullOrWhiteSpace(request.Category) ? null : request.Category.Trim(),
            IsActive = request.IsActive
        };

        _context.Crops.Add(crop);
        await _context.SaveChangesAsync();

        return Created($"/api/crops/{crop.CropId}", new
        {
            crop.CropId,
            crop.Name,
            crop.Category,
            crop.IsActive
        });
    }

    [Authorize(Roles = "Administrator")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CropRequest request)
    {
        var crop = await _context.Crops.FindAsync(id);
        if (crop == null) return NotFound();

        var normalized = request.Name.Trim().ToLower();
        if (await _context.Crops.AnyAsync(c => c.CropId != id && c.Name.ToLower() == normalized))
            return Conflict(new { message = "Another crop already uses this name." });

        crop.Name = request.Name.Trim();
        crop.Category = string.IsNullOrWhiteSpace(request.Category) ? null : request.Category.Trim();
        crop.IsActive = request.IsActive;
        await _context.SaveChangesAsync();

        return Ok(new { crop.CropId, crop.Name, crop.Category, crop.IsActive });
    }
}

public sealed class CropRequest
{
    [System.ComponentModel.DataAnnotations.Required, System.ComponentModel.DataAnnotations.StringLength(100)]
    public string Name { get; set; } = string.Empty;
    [System.ComponentModel.DataAnnotations.StringLength(100)]
    public string? Category { get; set; }
    public bool IsActive { get; set; } = true;
}
