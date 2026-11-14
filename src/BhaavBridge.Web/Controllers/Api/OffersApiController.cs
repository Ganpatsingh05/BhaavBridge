using BhaavBridge.Web.Data;
using BhaavBridge.Web.Models.Api;
using BhaavBridge.Web.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BhaavBridge.Web.Controllers.Api;

[ApiController]
[Route("api/offers")]
[Authorize(Roles = "Buyer,Farmer")]
public sealed class OffersApiController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public OffersApiController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [Authorize(Roles = "Buyer")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOfferRequest request)
    {
        var userId = _userManager.GetUserId(User);
        var buyer = await _context.BuyerProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
        if (buyer == null) return Forbid();

        var lot = await _context.ProduceLots.FirstOrDefaultAsync(l =>
            l.ProduceLotId == request.ProduceLotId && l.Status == ProduceLotStatus.Published);
        if (lot == null) return NotFound(new { message = "Published produce lot not found." });

        var duplicate = await _context.Offers.AnyAsync(o =>
            o.ProduceLotId == lot.ProduceLotId &&
            o.BuyerProfileId == buyer.BuyerProfileId &&
            (o.Status == OfferStatus.Pending || o.Status == OfferStatus.Countered));
        if (duplicate) return Conflict(new { message = "An active offer already exists for this lot." });

        var offer = new Offer
        {
            ProduceLotId = lot.ProduceLotId,
            BuyerProfileId = buyer.BuyerProfileId,
            CurrentPricePerQuintal = request.PricePerQuintal,
            QuantityQuintals = lot.QuantityQuintals,
            TransportationCost = request.TransportationCost,
            LoadingCost = request.LoadingCost,
            OtherFarmerCost = request.OtherFarmerCost,
            Message = request.Message?.Trim(),
            Status = OfferStatus.Pending
        };

        offer.Revisions.Add(new OfferRevision
        {
            RevisionNumber = 1,
            ProposedByUserId = userId!,
            ProposedPricePerQuintal = request.PricePerQuintal,
            ProposedQuantityQuintals = lot.QuantityQuintals,
            TransportationCost = request.TransportationCost,
            LoadingCost = request.LoadingCost,
            OtherFarmerCost = request.OtherFarmerCost,
            Message = request.Message?.Trim(),
            CreatedAtUtc = DateTime.UtcNow
        });

        _context.Offers.Add(offer);
        await _context.SaveChangesAsync();

        return Created($"/api/offers/{offer.OfferId}", new
        {
            offer.OfferId,
            offer.ProduceLotId,
            offer.CurrentPricePerQuintal,
            offer.QuantityQuintals,
            Status = offer.Status.ToString()
        });
    }

    [Authorize(Roles = "Buyer")]
    [HttpGet("mine")]
    public async Task<IActionResult> Mine()
    {
        var userId = _userManager.GetUserId(User);
        var buyer = await _context.BuyerProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
        if (buyer == null) return Forbid();

        var offers = await _context.Offers.AsNoTracking()
            .Where(o => o.BuyerProfileId == buyer.BuyerProfileId)
            .OrderByDescending(o => o.CreatedAtUtc)
            .Select(o => new
            {
                o.OfferId,
                o.ProduceLotId,
                CropName = o.ProduceLot.Crop.Name,
                o.CurrentPricePerQuintal,
                o.QuantityQuintals,
                o.TransportationCost,
                o.LoadingCost,
                o.OtherFarmerCost,
                Status = o.Status.ToString(),
                RevisionCount = o.Revisions.Count
            }).ToListAsync();

        return Ok(offers);
    }

    [Authorize(Roles = "Farmer")]
    [HttpGet("incoming")]
    public async Task<IActionResult> Incoming()
    {
        var userId = _userManager.GetUserId(User);
        var farmer = await _context.FarmerProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
        if (farmer == null) return Forbid();

        var offers = await _context.Offers.AsNoTracking()
            .Where(o => o.ProduceLot.FarmerProfileId == farmer.FarmerProfileId)
            .OrderByDescending(o => o.CreatedAtUtc)
            .Select(o => new
            {
                o.OfferId,
                o.ProduceLotId,
                CropName = o.ProduceLot.Crop.Name,
                BuyerBusiness = o.BuyerProfile.BusinessName,
                o.QuantityQuintals,
                o.CurrentPricePerQuintal,
                GrossValue = o.CurrentPricePerQuintal * o.QuantityQuintals,
                TotalFarmerCosts = o.TransportationCost + o.LoadingCost + o.OtherFarmerCost,
                NetDaam = (o.CurrentPricePerQuintal * o.QuantityQuintals) -
                          o.TransportationCost - o.LoadingCost - o.OtherFarmerCost,
                Status = o.Status.ToString()
            }).ToListAsync();

        return Ok(offers);
    }

    [Authorize(Roles = "Farmer")]
    [HttpPost("{id:int}/counter")]
    public async Task<IActionResult> Counter(int id, [FromBody] CounterOfferRequest request)
    {
        var userId = _userManager.GetUserId(User);
        var farmer = await _context.FarmerProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
        if (farmer == null) return Forbid();

        var offer = await _context.Offers
            .Include(o => o.Revisions)
            .Include(o => o.ProduceLot)
            .FirstOrDefaultAsync(o => o.OfferId == id &&
                o.ProduceLot.FarmerProfileId == farmer.FarmerProfileId);
        if (offer == null) return NotFound();

        if (offer.ProduceLot.Status != ProduceLotStatus.Published ||
            offer.Status != OfferStatus.Pending)
            return Conflict(new { message = "This offer can no longer be countered." });

        var nextRevision = offer.Revisions.Select(r => r.RevisionNumber).DefaultIfEmpty(0).Max() + 1;
        offer.Revisions.Add(new OfferRevision
        {
            OfferId = offer.OfferId,
            RevisionNumber = nextRevision,
            ProposedByUserId = userId!,
            ProposedPricePerQuintal = request.PricePerQuintal,
            ProposedQuantityQuintals = offer.QuantityQuintals,
            TransportationCost = offer.TransportationCost,
            LoadingCost = offer.LoadingCost,
            OtherFarmerCost = offer.OtherFarmerCost,
            Message = request.Message?.Trim(),
            CreatedAtUtc = DateTime.UtcNow
        });

        offer.CurrentPricePerQuintal = request.PricePerQuintal;
        offer.Message = request.Message?.Trim();
        offer.Status = OfferStatus.Countered;
        offer.UpdatedAtUtc = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return Ok(new { offer.OfferId, offer.CurrentPricePerQuintal, Status = offer.Status.ToString(), RevisionNumber = nextRevision });
    }
}
