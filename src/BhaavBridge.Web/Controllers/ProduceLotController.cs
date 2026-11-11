using BhaavBridge.Web.Models.Entities;
using BhaavBridge.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BhaavBridge.Web.Controllers;

/// <summary>
/// Manages harvest lots for Farmers.
/// Only the owning Farmer can create, edit, publish or close their own lots.
/// </summary>
[Authorize(Roles = "Farmer")]
public class ProduceLotController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public ProduceLotController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    /// <summary>Returns the FarmerProfile for the currently logged-in user,
    /// or null if not found.</summary>
    private async Task<FarmerProfile?> GetCurrentFarmerProfileAsync()
    {
        var userId = _userManager.GetUserId(User);
        return await _context.FarmerProfiles
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.UserId == userId);
    }

    /// <summary>Loads active crops as a SelectList for the create/edit form.</summary>
    private async Task<IEnumerable<SelectListItem>> BuildCropSelectListAsync(int? selectedCropId = null)
    {
        var crops = await _context.Crops
            .Where(c => c.IsActive)
            .OrderBy(c => c.Name)
            .ToListAsync();

        return crops.Select(c => new SelectListItem
        {
            Value = c.CropId.ToString(),
            Text = string.IsNullOrWhiteSpace(c.Category) ? c.Name : $"{c.Name} ({c.Category})",
            Selected = c.CropId == selectedCropId
        });
    }

    // ── Farmer: list own lots ─────────────────────────────────────────────────

    // GET: /ProduceLot
    public async Task<IActionResult> Index()
    {
        var profile = await GetCurrentFarmerProfileAsync();
        if (profile == null)
            return RedirectToAction("Farmer", "Profile");

        var lots = await _context.ProduceLots
            .Where(l => l.FarmerProfileId == profile.FarmerProfileId)
            .Include(l => l.Crop)
            .Include(l => l.FarmerProfile)
                .ThenInclude(fp => fp.User)
            .OrderByDescending(l => l.CreatedAtUtc)
            .Select(l => new ProduceLotSummaryViewModel
            {
                ProduceLotId = l.ProduceLotId,
                CropName = l.Crop.Name,
                Variety = l.Variety,
                QualityGrade = l.QualityGrade,
                QuantityQuintals = l.QuantityQuintals,
                HarvestDate = l.HarvestDate,
                District = l.District,
                State = l.State,
                AskingPricePerQuintal = l.AskingPricePerQuintal,
                Description = l.Description,
                FarmerName = l.FarmerProfile.User != null ? l.FarmerProfile.User.FullName : string.Empty,
                Status = l.Status.ToString(),
                CreatedAtUtc = l.CreatedAtUtc
            })
            .ToListAsync();

        return View(lots);
    }

    // ── Farmer: create ────────────────────────────────────────────────────────

    // GET: /ProduceLot/Create
    public async Task<IActionResult> Create()
    {
        var profile = await GetCurrentFarmerProfileAsync();
        if (profile == null)
            return RedirectToAction("Farmer", "Profile");

        var model = new ProduceLotFormViewModel
        {
            District = profile.District,
            State = profile.State,
            CropSelectList = await BuildCropSelectListAsync()
        };

        return View(model);
    }

    // POST: /ProduceLot/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProduceLotFormViewModel model)
    {
        // Always repopulate the select list (not bound from form).
        model.CropSelectList = await BuildCropSelectListAsync(model.CropId);

        if (!ModelState.IsValid)
            return View(model);

        var profile = await GetCurrentFarmerProfileAsync();
        if (profile == null)
            return RedirectToAction("Farmer", "Profile");

        // Verify the chosen crop is active.
        var cropExists = await _context.Crops
            .AnyAsync(c => c.CropId == model.CropId && c.IsActive);

        if (!cropExists)
        {
            ModelState.AddModelError(nameof(model.CropId), "Selected crop is not valid.");
            return View(model);
        }

        var lot = new ProduceLot
        {
            FarmerProfileId = profile.FarmerProfileId,
            CropId = model.CropId,
            Variety = string.IsNullOrWhiteSpace(model.Variety) ? null : model.Variety.Trim(),
            QualityGrade = string.IsNullOrWhiteSpace(model.QualityGrade) ? null : model.QualityGrade.Trim(),
            QuantityQuintals = model.QuantityQuintals,
            HarvestDate = model.HarvestDate,
            District = model.District.Trim(),
            State = model.State.Trim(),
            AskingPricePerQuintal = model.AskingPricePerQuintal,
            Description = string.IsNullOrWhiteSpace(model.Description) ? null : model.Description.Trim(),
            Status = ProduceLotStatus.Draft
        };

        _context.ProduceLots.Add(lot);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Harvest lot saved as Draft. Publish it when you're ready.";
        return RedirectToAction(nameof(Index));
    }

    // ── Farmer: edit ──────────────────────────────────────────────────────────

    // GET: /ProduceLot/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var profile = await GetCurrentFarmerProfileAsync();
        if (profile == null)
            return RedirectToAction("Farmer", "Profile");

        var lot = await _context.ProduceLots
            .FirstOrDefaultAsync(l => l.ProduceLotId == id &&
                                      l.FarmerProfileId == profile.FarmerProfileId);

        if (lot == null) return NotFound();

        // Only Draft lots may be edited.
        if (lot.Status != ProduceLotStatus.Draft)
        {
            TempData["ErrorMessage"] = "Only Draft lots can be edited.";
            return RedirectToAction(nameof(Index));
        }

        var model = new ProduceLotFormViewModel
        {
            CropId = lot.CropId,
            Variety = lot.Variety,
            QualityGrade = lot.QualityGrade,
            QuantityQuintals = lot.QuantityQuintals,
            HarvestDate = lot.HarvestDate,
            District = lot.District,
            State = lot.State,
            AskingPricePerQuintal = lot.AskingPricePerQuintal,
            Description = lot.Description,
            CropSelectList = await BuildCropSelectListAsync(lot.CropId)
        };

        ViewData["ProduceLotId"] = id;
        return View(model);
    }

    // POST: /ProduceLot/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ProduceLotFormViewModel model)
    {
        model.CropSelectList = await BuildCropSelectListAsync(model.CropId);
        ViewData["ProduceLotId"] = id;

        if (!ModelState.IsValid)
            return View(model);

        var profile = await GetCurrentFarmerProfileAsync();
        if (profile == null)
            return RedirectToAction("Farmer", "Profile");

        var lot = await _context.ProduceLots
            .FirstOrDefaultAsync(l => l.ProduceLotId == id &&
                                      l.FarmerProfileId == profile.FarmerProfileId);

        if (lot == null) return NotFound();

        if (lot.Status != ProduceLotStatus.Draft)
        {
            TempData["ErrorMessage"] = "Only Draft lots can be edited.";
            return RedirectToAction(nameof(Index));
        }

        var cropExists = await _context.Crops
            .AnyAsync(c => c.CropId == model.CropId && c.IsActive);

        if (!cropExists)
        {
            ModelState.AddModelError(nameof(model.CropId), "Selected crop is not valid.");
            return View(model);
        }

        lot.CropId = model.CropId;
        lot.Variety = string.IsNullOrWhiteSpace(model.Variety) ? null : model.Variety.Trim();
        lot.QualityGrade = string.IsNullOrWhiteSpace(model.QualityGrade) ? null : model.QualityGrade.Trim();
        lot.QuantityQuintals = model.QuantityQuintals;
        lot.HarvestDate = model.HarvestDate;
        lot.District = model.District.Trim();
        lot.State = model.State.Trim();
        lot.AskingPricePerQuintal = model.AskingPricePerQuintal;
        lot.Description = string.IsNullOrWhiteSpace(model.Description) ? null : model.Description.Trim();

        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Harvest lot updated.";
        return RedirectToAction(nameof(Index));
    }

    // ── Farmer: publish ───────────────────────────────────────────────────────

    // POST: /ProduceLot/Publish/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Publish(int id)
    {
        var profile = await GetCurrentFarmerProfileAsync();
        if (profile == null)
            return RedirectToAction("Farmer", "Profile");

        var lot = await _context.ProduceLots
            .FirstOrDefaultAsync(l => l.ProduceLotId == id &&
                                      l.FarmerProfileId == profile.FarmerProfileId);

        if (lot == null) return NotFound();

        if (lot.Status != ProduceLotStatus.Draft)
        {
            TempData["ErrorMessage"] = "Only Draft lots can be published.";
            return RedirectToAction(nameof(Index));
        }

        lot.Status = ProduceLotStatus.Published;
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Lot published! Buyers can now see it on the marketplace.";
        return RedirectToAction(nameof(Index));
    }

    // ── Farmer: close ─────────────────────────────────────────────────────────

    // POST: /ProduceLot/Close/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Close(int id)
    {
        var profile = await GetCurrentFarmerProfileAsync();
        if (profile == null)
            return RedirectToAction("Farmer", "Profile");

        var lot = await _context.ProduceLots
            .FirstOrDefaultAsync(l => l.ProduceLotId == id &&
                                      l.FarmerProfileId == profile.FarmerProfileId);

        if (lot == null) return NotFound();

        if (lot.Status == ProduceLotStatus.Closed)
        {
            TempData["ErrorMessage"] = "Lot is already closed.";
            return RedirectToAction(nameof(Index));
        }

        lot.Status = ProduceLotStatus.Closed;
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Lot closed. It is no longer visible on the marketplace.";
        return RedirectToAction(nameof(Index));
    }

    // ── Farmer: detail view ───────────────────────────────────────────────────

    // GET: /ProduceLot/Details/5
    // Farmer can view any of their own lots (Draft, Published or Closed).
    public async Task<IActionResult> Details(int id)
    {
        var profile = await GetCurrentFarmerProfileAsync();
        if (profile == null)
            return RedirectToAction("Farmer", "Profile");

        var lot = await _context.ProduceLots
            .Include(l => l.Crop)
            .Include(l => l.FarmerProfile)
                .ThenInclude(fp => fp.User)
            .FirstOrDefaultAsync(l => l.ProduceLotId == id &&
                                      l.FarmerProfileId == profile.FarmerProfileId);

        if (lot == null) return NotFound();

        var vm = new ProduceLotSummaryViewModel
        {
            ProduceLotId = lot.ProduceLotId,
            CropName = lot.Crop.Name,
            Variety = lot.Variety,
            QualityGrade = lot.QualityGrade,
            QuantityQuintals = lot.QuantityQuintals,
            HarvestDate = lot.HarvestDate,
            District = lot.District,
            State = lot.State,
            AskingPricePerQuintal = lot.AskingPricePerQuintal,
            Description = lot.Description,
            FarmerName = lot.FarmerProfile.User?.FullName ?? string.Empty,
            Status = lot.Status.ToString(),
            CreatedAtUtc = lot.CreatedAtUtc
        };

        return View(vm);
    }
}
