using BhaavBridge.Web.Models.Entities;
using BhaavBridge.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BhaavBridge.Web.Controllers;

/// <summary>
/// Administrator-only crop catalogue management.
/// Farmers and Buyers may read the crop list via JSON (used to populate selects).
/// </summary>
[Authorize(Roles = "Administrator")]
public class CropController : Controller
{
    private readonly ApplicationDbContext _context;

    public CropController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: /Crop
    public async Task<IActionResult> Index()
    {
        var crops = await _context.Crops
            .OrderBy(c => c.Name)
            .ToListAsync();

        return View(crops);
    }

    // GET: /Crop/Create
    public IActionResult Create()
    {
        return View(new CropViewModel());
    }

    // POST: /Crop/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CropViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        // Check for a duplicate name (case-insensitive).
        bool duplicate = await _context.Crops
            .AnyAsync(c => c.Name.ToLower() == model.Name.ToLower().Trim());

        if (duplicate)
        {
            ModelState.AddModelError(nameof(model.Name),
                "A crop with this name already exists.");
            return View(model);
        }

        var crop = new Crop
        {
            Name = model.Name.Trim(),
            Category = string.IsNullOrWhiteSpace(model.Category)
                ? null
                : model.Category.Trim(),
            IsActive = model.IsActive
        };

        _context.Crops.Add(crop);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = $"Crop '{crop.Name}' created successfully.";
        return RedirectToAction(nameof(Index));
    }

    // GET: /Crop/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var crop = await _context.Crops.FindAsync(id);
        if (crop == null) return NotFound();

        var model = new CropViewModel
        {
            Name = crop.Name,
            Category = crop.Category,
            IsActive = crop.IsActive
        };

        ViewData["CropId"] = id;
        return View(model);
    }

    // POST: /Crop/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CropViewModel model)
    {
        var crop = await _context.Crops.FindAsync(id);
        if (crop == null) return NotFound();

        if (!ModelState.IsValid)
        {
            ViewData["CropId"] = id;
            return View(model);
        }

        // Check for a duplicate name on a *different* crop.
        bool duplicate = await _context.Crops
            .AnyAsync(c => c.CropId != id &&
                           c.Name.ToLower() == model.Name.ToLower().Trim());

        if (duplicate)
        {
            ModelState.AddModelError(nameof(model.Name),
                "Another crop with this name already exists.");
            ViewData["CropId"] = id;
            return View(model);
        }

        crop.Name = model.Name.Trim();
        crop.Category = string.IsNullOrWhiteSpace(model.Category)
            ? null
            : model.Category.Trim();
        crop.IsActive = model.IsActive;

        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = $"Crop '{crop.Name}' updated successfully.";
        return RedirectToAction(nameof(Index));
    }
}
