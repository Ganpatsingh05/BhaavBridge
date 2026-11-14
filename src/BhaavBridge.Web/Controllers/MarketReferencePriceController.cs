using BhaavBridge.Web.Models.Entities;
using BhaavBridge.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BhaavBridge.Web.Controllers;

[Authorize(Roles = "Farmer,Buyer,Administrator")]
public sealed class MarketReferencePriceController : Controller
{
    private readonly ApplicationDbContext _context;

    public MarketReferencePriceController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int? cropId, string? district, string? state)
    {
        var query = _context.MarketReferencePrices
            .AsNoTracking()
            .Include(x => x.Crop)
            .AsQueryable();

        if (cropId.HasValue && cropId.Value > 0)
            query = query.Where(x => x.CropId == cropId.Value);

        if (!string.IsNullOrWhiteSpace(district))
            query = query.Where(x => x.District.Contains(district.Trim()));

        if (!string.IsNullOrWhiteSpace(state))
            query = query.Where(x => x.State.Contains(state.Trim()));

        var rows = await query
            .OrderByDescending(x => x.EffectiveDate)
            .ThenBy(x => x.Crop.Name)
            .ToListAsync();

        ViewBag.CropOptions = new SelectList(
            await _context.Crops.Where(c => c.IsActive).OrderBy(c => c.Name).ToListAsync(),
            "CropId", "Name", cropId);
        ViewBag.CropId = cropId;
        ViewBag.District = district;
        ViewBag.State = state;

        return View(rows);
    }

    [Authorize(Roles = "Administrator")]
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var model = new MarketReferencePriceFormViewModel
        {
            CropOptions = await BuildCropOptionsAsync()
        };
        return View(model);
    }

    [Authorize(Roles = "Administrator")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MarketReferencePriceFormViewModel model)
    {
        model.CropOptions = await BuildCropOptionsAsync(model.CropId);
        ValidatePriceRange(model);

        if (!ModelState.IsValid)
            return View(model);

        var cropExists = await _context.Crops.AnyAsync(c => c.CropId == model.CropId && c.IsActive);
        if (!cropExists)
        {
            ModelState.AddModelError(nameof(model.CropId), "Select an active crop.");
            return View(model);
        }

        _context.MarketReferencePrices.Add(new MarketReferencePrice
        {
            CropId = model.CropId,
            MarketName = model.MarketName.Trim(),
            District = model.District.Trim(),
            State = model.State.Trim(),
            EffectiveDate = model.EffectiveDate,
            MinimumPrice = model.MinimumPrice,
            MaximumPrice = model.MaximumPrice,
            ModalPrice = model.ModalPrice,
            SourceName = model.SourceName.Trim(),
            SourceUrl = string.IsNullOrWhiteSpace(model.SourceUrl) ? null : model.SourceUrl.Trim(),
            RecordedAtUtc = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = "Market reference price recorded.";
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Administrator")]
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var row = await _context.MarketReferencePrices.FindAsync(id);
        if (row == null) return NotFound();

        var model = new MarketReferencePriceFormViewModel
        {
            MarketReferencePriceId = row.MarketReferencePriceId,
            CropId = row.CropId,
            MarketName = row.MarketName,
            District = row.District,
            State = row.State,
            EffectiveDate = row.EffectiveDate,
            MinimumPrice = row.MinimumPrice,
            MaximumPrice = row.MaximumPrice,
            ModalPrice = row.ModalPrice,
            SourceName = row.SourceName,
            SourceUrl = row.SourceUrl,
            CropOptions = await BuildCropOptionsAsync(row.CropId)
        };
        return View(model);
    }

    [Authorize(Roles = "Administrator")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, MarketReferencePriceFormViewModel model)
    {
        var row = await _context.MarketReferencePrices.FindAsync(id);
        if (row == null) return NotFound();

        model.CropOptions = await BuildCropOptionsAsync(model.CropId);
        ValidatePriceRange(model);

        if (!ModelState.IsValid)
            return View(model);

        row.CropId = model.CropId;
        row.MarketName = model.MarketName.Trim();
        row.District = model.District.Trim();
        row.State = model.State.Trim();
        row.EffectiveDate = model.EffectiveDate;
        row.MinimumPrice = model.MinimumPrice;
        row.MaximumPrice = model.MaximumPrice;
        row.ModalPrice = model.ModalPrice;
        row.SourceName = model.SourceName.Trim();
        row.SourceUrl = string.IsNullOrWhiteSpace(model.SourceUrl) ? null : model.SourceUrl.Trim();

        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = "Market reference price updated.";
        return RedirectToAction(nameof(Index));
    }

    private async Task<IEnumerable<SelectListItem>> BuildCropOptionsAsync(int? selected = null)
    {
        return await _context.Crops
            .Where(c => c.IsActive)
            .OrderBy(c => c.Name)
            .Select(c => new SelectListItem
            {
                Value = c.CropId.ToString(),
                Text = c.Name,
                Selected = c.CropId == selected
            })
            .ToListAsync();
    }

    private void ValidatePriceRange(MarketReferencePriceFormViewModel model)
    {
        if (model.MinimumPrice.HasValue && model.MaximumPrice.HasValue &&
            model.MinimumPrice.Value > model.MaximumPrice.Value)
        {
            ModelState.AddModelError(nameof(model.MaximumPrice),
                "Maximum price must be greater than or equal to minimum price.");
        }

        if (model.MinimumPrice.HasValue && model.ModalPrice < model.MinimumPrice.Value)
            ModelState.AddModelError(nameof(model.ModalPrice),
                "Modal price cannot be below the minimum price.");

        if (model.MaximumPrice.HasValue && model.ModalPrice > model.MaximumPrice.Value)
            ModelState.AddModelError(nameof(model.ModalPrice),
                "Modal price cannot exceed the maximum price.");
    }
}
