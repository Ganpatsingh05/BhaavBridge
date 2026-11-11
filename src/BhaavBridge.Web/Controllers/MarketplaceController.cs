using BhaavBridge.Web.Models.Entities;
using BhaavBridge.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BhaavBridge.Web.Controllers;

/// <summary>
/// Buyer-facing marketplace: browse, search and filter Published ProduceLots.
/// Accessible by authenticated Buyers only.
/// </summary>
[Authorize(Roles = "Buyer")]
public class MarketplaceController : Controller
{
    private readonly ApplicationDbContext _context;

    public MarketplaceController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: /Marketplace
    // GET: /Marketplace?CropId=3&District=Nashik&MinPrice=1000&MaxPrice=3000
    public async Task<IActionResult> Index(MarketplaceFilterViewModel filter)
    {
        // Build the base query — only Published lots are shown to buyers.
        var query = _context.ProduceLots
            .Where(l => l.Status == ProduceLotStatus.Published)
            .Include(l => l.Crop)
            .Include(l => l.FarmerProfile)
                .ThenInclude(fp => fp.User)
            .AsQueryable();

        // Apply optional filters.
        if (filter.CropId.HasValue && filter.CropId > 0)
            query = query.Where(l => l.CropId == filter.CropId);

        if (!string.IsNullOrWhiteSpace(filter.District))
            query = query.Where(l => l.District.ToLower().Contains(filter.District.ToLower().Trim()));

        if (!string.IsNullOrWhiteSpace(filter.State))
            query = query.Where(l => l.State.ToLower().Contains(filter.State.ToLower().Trim()));

        if (filter.MinQuantity.HasValue)
            query = query.Where(l => l.QuantityQuintals >= filter.MinQuantity.Value);

        if (filter.MaxQuantity.HasValue)
            query = query.Where(l => l.QuantityQuintals <= filter.MaxQuantity.Value);

        if (filter.MinPrice.HasValue)
            query = query.Where(l => l.AskingPricePerQuintal >= filter.MinPrice.Value);

        if (filter.MaxPrice.HasValue)
            query = query.Where(l => l.AskingPricePerQuintal <= filter.MaxPrice.Value);

        // Project to summary view model.
        var lots = await query
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
                FarmerName = l.FarmerProfile.User != null
                    ? l.FarmerProfile.User.FullName
                    : string.Empty,
                Status = l.Status.ToString(),
                CreatedAtUtc = l.CreatedAtUtc
            })
            .ToListAsync();

        // Populate crop drop-down (only active crops that have at least one Published lot).
        var cropSelectList = await _context.Crops
            .Where(c => c.IsActive)
            .OrderBy(c => c.Name)
            .Select(c => new SelectListItem
            {
                Value = c.CropId.ToString(),
                Text = c.Name,
                Selected = c.CropId == filter.CropId
            })
            .ToListAsync();

        filter.Lots = lots;
        filter.CropSelectList = cropSelectList;

        return View(filter);
    }

    // GET: /Marketplace/Details/5  — read-only lot detail for buyers.
    public async Task<IActionResult> Details(int id)
    {
        var lot = await _context.ProduceLots
            .Include(l => l.Crop)
            .Include(l => l.FarmerProfile)
                .ThenInclude(fp => fp.User)
            .FirstOrDefaultAsync(l => l.ProduceLotId == id &&
                                      l.Status == ProduceLotStatus.Published);

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
