
using BhaavBridge.Web.Models.Entities;
using BhaavBridge.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BhaavBridge.Web.Controllers;

[Authorize]
public class OfferController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public OfferController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET: /Offer/Create/5
    [Authorize(Roles = "Buyer")]
    [HttpGet]
    public async Task<IActionResult> Create(int produceLotId)
    {
        var buyer = await GetCurrentBuyerProfileAsync();

        if (buyer == null)
            return Forbid();

        var lot = await GetPublishedLotAsync(produceLotId);

        if (lot == null)
            return NotFound();

        ViewBag.Lot = lot;

        var model = new CreateOfferViewModel
        {
            ProduceLotId = lot.ProduceLotId,
            PricePerQuintal = lot.AskingPricePerQuintal
        };

        return View(model);
    }

    // POST: /Offer/Create
    [Authorize(Roles = "Buyer")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateOfferViewModel model)
    {
        var buyer = await GetCurrentBuyerProfileAsync();

        if (buyer == null)
            return Forbid();

        var lot = await GetPublishedLotAsync(model.ProduceLotId);

        if (lot == null)
            return NotFound();

        ViewBag.Lot = lot;

        if (!ModelState.IsValid)
            return View(model);

        var hasActiveOffer = await _context.Offers.AnyAsync(o =>
            o.ProduceLotId == lot.ProduceLotId &&
            o.BuyerProfileId == buyer.BuyerProfileId &&
            (o.Status == OfferStatus.Pending ||
             o.Status == OfferStatus.Countered));

        if (hasActiveOffer)
        {
            ModelState.AddModelError(
                string.Empty,
                "You already have an active offer for this lot.");

            return View(model);
        }

        var userId = _userManager.GetUserId(User);

        if (userId == null)
            return Forbid();

        var offer = new Offer
        {
            ProduceLotId = lot.ProduceLotId,
            BuyerProfileId = buyer.BuyerProfileId,
            CurrentPricePerQuintal = model.PricePerQuintal,
            QuantityQuintals = lot.QuantityQuintals,
            TransportationCost = model.TransportationCost,
            LoadingCost = model.LoadingCost,
            OtherFarmerCost = model.OtherFarmerCost,
            Message = string.IsNullOrWhiteSpace(model.Message)
                ? null
                : model.Message.Trim(),
            Status = OfferStatus.Pending
        };

        var revision = new OfferRevision
        {
            RevisionNumber = 1,
            ProposedByUserId = userId,
            ProposedPricePerQuintal = model.PricePerQuintal,
            ProposedQuantityQuintals = lot.QuantityQuintals,
            TransportationCost = model.TransportationCost,
            LoadingCost = model.LoadingCost,
            OtherFarmerCost = model.OtherFarmerCost,
            Message = offer.Message,
            CreatedAtUtc = DateTime.UtcNow
        };

        offer.Revisions.Add(revision);
        _context.Offers.Add(offer);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError(
                string.Empty,
                "The offer could not be submitted. An active offer may already exist for this lot.");

            return View(model);
        }

        TempData["SuccessMessage"] =
            "Your offer has been submitted successfully.";

        return RedirectToAction(
            "Details",
            "Marketplace",
            new { id = lot.ProduceLotId });
    }

    // GET: /Offer/MyOffers
    [Authorize(Roles = "Buyer")]
    [HttpGet]
    public async Task<IActionResult> MyOffers()
    {
        var buyer = await GetCurrentBuyerProfileAsync();

        if (buyer == null)
            return Forbid();

        var offers = await _context.Offers
            .Where(o => o.BuyerProfileId == buyer.BuyerProfileId)
            .Include(o => o.ProduceLot)
                .ThenInclude(l => l.Crop)
            .Include(o => o.ProduceLot)
                .ThenInclude(l => l.FarmerProfile)
                    .ThenInclude(f => f.User)
            .Include(o => o.Revisions)
                .ThenInclude(r => r.ProposedByUser)
            .OrderByDescending(o => o.CreatedAtUtc)
            .ToListAsync();

        var result = offers.Select(o =>
        {
            var revisions = o.Revisions
                .OrderBy(r => r.RevisionNumber)
                .Select(r => new BuyerOfferRevisionViewModel
                {
                    RevisionNumber = r.RevisionNumber,

                    ProposedByName = r.ProposedByUserId == buyer.UserId
                        ? "You (Buyer)"
                        : "Farmer",

                    ProposedPricePerQuintal =
                        r.ProposedPricePerQuintal,

                    ProposedQuantityQuintals =
                        r.ProposedQuantityQuintals,

                    TransportationCost = r.TransportationCost,
                    LoadingCost = r.LoadingCost,
                    OtherFarmerCost = r.OtherFarmerCost,

                    Message = r.Message,
                    CreatedAtUtc = r.CreatedAtUtc
                })
                .ToList();

            var totalCosts =
                o.TransportationCost +
                o.LoadingCost +
                o.OtherFarmerCost;

            var grossValue =
                o.CurrentPricePerQuintal * o.QuantityQuintals;

            return new BuyerOfferViewModel
            {
                OfferId = o.OfferId,
                ProduceLotId = o.ProduceLotId,

                CropName = o.ProduceLot.Crop.Name,
                FarmerName = o.ProduceLot.FarmerProfile.User.FullName,

                QuantityQuintals = o.QuantityQuintals,

                OriginalPricePerQuintal =
                    revisions.FirstOrDefault()?.ProposedPricePerQuintal
                    ?? o.CurrentPricePerQuintal,

                CurrentPricePerQuintal = o.CurrentPricePerQuintal,

                TransportationCost = o.TransportationCost,
                LoadingCost = o.LoadingCost,
                OtherFarmerCost = o.OtherFarmerCost,

                GrossValue = grossValue,
                TotalEstimatedCosts = totalCosts,
                NetDaam = grossValue - totalCosts,

                Message = o.Message,
                Status = o.Status.ToString(),
                CreatedAtUtc = o.CreatedAtUtc,

                Revisions = revisions
            };
        }).ToList();

        return View(result);
    }

    // GET: /Offer/Incoming
    [Authorize(Roles = "Farmer")]
    [HttpGet]
    public async Task<IActionResult> Incoming()
    {
        var userId = _userManager.GetUserId(User);

        if (userId == null)
            return Forbid();

        var farmer = await _context.FarmerProfiles
            .FirstOrDefaultAsync(f => f.UserId == userId);

        if (farmer == null)
            return RedirectToAction("Farmer", "Profile");

        var offers = await _context.Offers
            .Where(o => o.ProduceLot.FarmerProfileId ==
                        farmer.FarmerProfileId)
            .Include(o => o.ProduceLot)
                .ThenInclude(l => l.Crop)
            .Include(o => o.BuyerProfile)
                .ThenInclude(b => b.User)
            .OrderByDescending(o => o.CreatedAtUtc)
            .Select(o => new FarmerIncomingOfferViewModel
            {
                OfferId = o.OfferId,
                ProduceLotId = o.ProduceLotId,
                CropName = o.ProduceLot.Crop.Name,
                BuyerBusinessName = o.BuyerProfile.BusinessName,
                BuyerContactPerson = o.BuyerProfile.ContactPerson,
                BuyerFullName = o.BuyerProfile.User.FullName,
                QuantityQuintals = o.QuantityQuintals,
                OfferPricePerQuintal = o.CurrentPricePerQuintal,
                TransportationCost = o.TransportationCost,
                LoadingCost = o.LoadingCost,
                OtherFarmerCost = o.OtherFarmerCost,

                GrossValue =
                    o.CurrentPricePerQuintal * o.QuantityQuintals,

                TotalEstimatedCosts =
                    o.TransportationCost +
                    o.LoadingCost +
                    o.OtherFarmerCost,

                NetDaam =
                    (o.CurrentPricePerQuintal * o.QuantityQuintals) -
                    o.TransportationCost -
                    o.LoadingCost -
                    o.OtherFarmerCost,

                NetDaamPerQuintal =
                    ((o.CurrentPricePerQuintal * o.QuantityQuintals) -
                    o.TransportationCost -
                    o.LoadingCost -
                    o.OtherFarmerCost) / o.QuantityQuintals,

                Message = o.Message,
                Status = o.Status.ToString(),
                CreatedAtUtc = o.CreatedAtUtc
            })
            .ToListAsync();

        return View(offers);
    }

    // POST: /Offer/Accept/1
    [Authorize(Roles = "Farmer")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Accept(int id)
    {
        var userId = _userManager.GetUserId(User);

        if (userId == null)
            return Forbid();

        var farmer = await _context.FarmerProfiles
            .FirstOrDefaultAsync(f => f.UserId == userId);

        if (farmer == null)
            return Forbid();

        await using var transaction =
            await _context.Database.BeginTransactionAsync(
                System.Data.IsolationLevel.Serializable);

        try
        {
            var offer = await _context.Offers
                .Include(o => o.ProduceLot)
                .FirstOrDefaultAsync(o =>
                    o.OfferId == id &&
                    o.ProduceLot.FarmerProfileId ==
                    farmer.FarmerProfileId);

            if (offer == null)
                return NotFound();

            if (offer.Status != OfferStatus.Pending)
            {
                TempData["ErrorMessage"] =
                    "This offer can no longer be accepted.";

                return RedirectToAction(nameof(Incoming));
            }

            if (offer.ProduceLot.Status != ProduceLotStatus.Published)
            {
                TempData["ErrorMessage"] =
                    "This produce lot is no longer available.";

                return RedirectToAction(nameof(Incoming));
            }

            var anotherAcceptedOffer = await _context.Offers
                .AnyAsync(o =>
                    o.ProduceLotId == offer.ProduceLotId &&
                    o.OfferId != offer.OfferId &&
                    o.Status == OfferStatus.Accepted);

            if (anotherAcceptedOffer)
            {
                TempData["ErrorMessage"] =
                    "Another offer has already been accepted for this lot.";

                return RedirectToAction(nameof(Incoming));
            }

            var existingOrder = await _context.Orders
                .AnyAsync(o => o.OfferId == offer.OfferId);

            if (existingOrder)
            {
                TempData["ErrorMessage"] =
                    "An order already exists for this offer.";

                return RedirectToAction(nameof(Incoming));
            }

            var grossValue =
                offer.CurrentPricePerQuintal * offer.QuantityQuintals;

            var totalCosts =
                offer.TransportationCost +
                offer.LoadingCost +
                offer.OtherFarmerCost;

            var now = DateTime.UtcNow;

            var order = new Order
            {
                OfferId = offer.OfferId,
                AgreedQuantityQuintals = offer.QuantityQuintals,
                AgreedPricePerQuintal = offer.CurrentPricePerQuintal,
                GrossValue = grossValue,
                TransportationCostSnapshot = offer.TransportationCost,
                LoadingCostSnapshot = offer.LoadingCost,
                OtherFarmerCostSnapshot = offer.OtherFarmerCost,
                TotalFarmerCostsSnapshot = totalCosts,
                EstimatedNetReturnSnapshot = grossValue - totalCosts,
                Status = OrderStatus.Placed,
                CreatedAtUtc = now,
                UpdatedAtUtc = now
            };

            offer.Status = OfferStatus.Accepted;
            offer.UpdatedAtUtc = now;

            offer.ProduceLot.Status = ProduceLotStatus.Closed;
            offer.ProduceLot.UpdatedAtUtc = now;

            _context.Orders.Add(order);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            TempData["SuccessMessage"] =
                "Offer accepted successfully. The produce lot has been closed and an order has been created.";

            return RedirectToAction(nameof(Incoming));
        }
        catch (DbUpdateException)
        {
            await transaction.RollbackAsync();

            TempData["ErrorMessage"] =
                "The offer could not be accepted. Please try again.";

            return RedirectToAction(nameof(Incoming));
        }
    }

    // POST: /Offer/Reject/1
    [Authorize(Roles = "Farmer")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reject(int id)
    {
        var userId = _userManager.GetUserId(User);

        if (userId == null)
            return Forbid();

        var farmer = await _context.FarmerProfiles
            .FirstOrDefaultAsync(f => f.UserId == userId);

        if (farmer == null)
            return Forbid();

        var offer = await _context.Offers
            .Include(o => o.ProduceLot)
            .FirstOrDefaultAsync(o =>
                o.OfferId == id &&
                o.ProduceLot.FarmerProfileId ==
                farmer.FarmerProfileId);

        if (offer == null)
            return NotFound();

        if (offer.Status != OfferStatus.Pending)
        {
            TempData["ErrorMessage"] =
                "This offer can no longer be rejected.";

            return RedirectToAction(nameof(Incoming));
        }

        if (offer.ProduceLot.Status != ProduceLotStatus.Published)
        {
            TempData["ErrorMessage"] =
                "This produce lot is no longer available.";

            return RedirectToAction(nameof(Incoming));
        }

        offer.Status = OfferStatus.Rejected;
        offer.UpdatedAtUtc = DateTime.UtcNow;

        try
        {
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Offer rejected successfully.";
        }
        catch (DbUpdateException)
        {
            TempData["ErrorMessage"] =
                "The offer could not be rejected. Please try again.";
        }

        return RedirectToAction(nameof(Incoming));
    }


    // POST: /Offer/AcceptCounter/1
    [Authorize(Roles = "Buyer")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AcceptCounter(int id)
    {
        var buyer = await GetCurrentBuyerProfileAsync();

        if (buyer == null)
            return Forbid();

        await using var transaction =
            await _context.Database.BeginTransactionAsync(
                System.Data.IsolationLevel.Serializable);

        try
        {
            var offer = await _context.Offers
                .Include(o => o.ProduceLot)
                    .ThenInclude(l => l.FarmerProfile)
                .Include(o => o.Revisions)
                .FirstOrDefaultAsync(o =>
                    o.OfferId == id &&
                    o.BuyerProfileId == buyer.BuyerProfileId);

            if (offer == null)
                return NotFound();

            if (offer.Status != OfferStatus.Countered)
            {
                TempData["ErrorMessage"] =
                    "This offer does not have an active counteroffer.";

                return RedirectToAction(nameof(MyOffers));
            }

            if (offer.ProduceLot.Status != ProduceLotStatus.Published)
            {
                TempData["ErrorMessage"] =
                    "This produce lot is no longer available.";

                return RedirectToAction(nameof(MyOffers));
            }

            var latestRevision = offer.Revisions
                .OrderByDescending(r => r.RevisionNumber)
                .FirstOrDefault();

            if (latestRevision == null ||
                latestRevision.ProposedByUserId != offer.ProduceLot.FarmerProfile.UserId)
            {
                TempData["ErrorMessage"] =
                    "A valid farmer counteroffer could not be found.";

                return RedirectToAction(nameof(MyOffers));
            }

            var anotherAcceptedOffer = await _context.Offers
                .AnyAsync(o =>
                    o.ProduceLotId == offer.ProduceLotId &&
                    o.OfferId != offer.OfferId &&
                    o.Status == OfferStatus.Accepted);

            if (anotherAcceptedOffer)
            {
                TempData["ErrorMessage"] =
                    "Another offer has already been accepted for this lot.";

                return RedirectToAction(nameof(MyOffers));
            }

            var existingOrder = await _context.Orders
                .AnyAsync(o => o.OfferId == offer.OfferId);

            if (existingOrder)
            {
                TempData["ErrorMessage"] =
                    "An order already exists for this offer.";

                return RedirectToAction(nameof(MyOffers));
            }

            var grossValue =
                offer.CurrentPricePerQuintal * offer.QuantityQuintals;

            var totalCosts =
                offer.TransportationCost +
                offer.LoadingCost +
                offer.OtherFarmerCost;

            var now = DateTime.UtcNow;

            var order = new Order
            {
                OfferId = offer.OfferId,
                AgreedQuantityQuintals = offer.QuantityQuintals,
                AgreedPricePerQuintal = offer.CurrentPricePerQuintal,
                GrossValue = grossValue,
                TransportationCostSnapshot = offer.TransportationCost,
                LoadingCostSnapshot = offer.LoadingCost,
                OtherFarmerCostSnapshot = offer.OtherFarmerCost,
                TotalFarmerCostsSnapshot = totalCosts,
                EstimatedNetReturnSnapshot = grossValue - totalCosts,
                Status = OrderStatus.Placed,
                CreatedAtUtc = now,
                UpdatedAtUtc = now
            };

            offer.Status = OfferStatus.Accepted;
            offer.UpdatedAtUtc = now;

            offer.ProduceLot.Status = ProduceLotStatus.Closed;
            offer.ProduceLot.UpdatedAtUtc = now;

            _context.Orders.Add(order);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            TempData["SuccessMessage"] =
                "Farmer counteroffer accepted. Your order has been created.";

            return RedirectToAction(nameof(MyOffers));
        }
        catch (DbUpdateException)
        {
            await transaction.RollbackAsync();

            TempData["ErrorMessage"] =
                "The counteroffer could not be accepted. Please try again.";

            return RedirectToAction(nameof(MyOffers));
        }
    }

    // POST: /Offer/RejectCounter/1
    [Authorize(Roles = "Buyer")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RejectCounter(int id)
    {
        var buyer = await GetCurrentBuyerProfileAsync();

        if (buyer == null)
            return Forbid();

        var offer = await _context.Offers
    .Include(o => o.ProduceLot)
        .ThenInclude(p => p.FarmerProfile)
    .Include(o => o.Revisions)
            .FirstOrDefaultAsync(o =>
                o.OfferId == id &&
                o.BuyerProfileId == buyer.BuyerProfileId);

        if (offer == null)
            return NotFound();

        if (offer.Status != OfferStatus.Countered)
        {
            TempData["ErrorMessage"] =
                "This offer does not have an active counteroffer.";

            return RedirectToAction(nameof(MyOffers));
        }

        if (offer.ProduceLot.Status != ProduceLotStatus.Published)
        {
            TempData["ErrorMessage"] =
                "This produce lot is no longer available.";

            return RedirectToAction(nameof(MyOffers));
        }

        var latestRevision = offer.Revisions
            .OrderByDescending(r => r.RevisionNumber)
            .FirstOrDefault();

        if (latestRevision == null ||
            latestRevision.ProposedByUserId != offer.ProduceLot.FarmerProfile.UserId)
        {
            TempData["ErrorMessage"] =
                "A valid farmer counteroffer could not be found.";

            return RedirectToAction(nameof(MyOffers));
        }

        offer.Status = OfferStatus.Rejected;
        offer.UpdatedAtUtc = DateTime.UtcNow;

        try
        {
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Farmer counteroffer rejected successfully. The lot remains available.";
        }
        catch (DbUpdateException)
        {
            TempData["ErrorMessage"] =
                "The counteroffer could not be rejected. Please try again.";
        }

        return RedirectToAction(nameof(MyOffers));
    }

    // GET: /Offer/Counter/1
    [Authorize(Roles = "Farmer")]
    [HttpGet]
    public async Task<IActionResult> Counter(int id)
    {
        var userId = _userManager.GetUserId(User);

        if (userId == null)
            return Forbid();

        var farmer = await _context.FarmerProfiles
            .FirstOrDefaultAsync(f => f.UserId == userId);

        if (farmer == null)
            return Forbid();

        var offer = await _context.Offers
            .Include(o => o.ProduceLot)
                .ThenInclude(l => l.Crop)
            .FirstOrDefaultAsync(o =>
                o.OfferId == id &&
                o.ProduceLot.FarmerProfileId ==
                farmer.FarmerProfileId);

        if (offer == null)
            return NotFound();

        if (offer.Status != OfferStatus.Pending)
        {
            TempData["ErrorMessage"] =
                "This offer can no longer be countered.";

            return RedirectToAction(nameof(Incoming));
        }

        if (offer.ProduceLot.Status != ProduceLotStatus.Published)
        {
            TempData["ErrorMessage"] =
                "This produce lot is no longer available.";

            return RedirectToAction(nameof(Incoming));
        }

        ViewBag.CropName = offer.ProduceLot.Crop.Name;
        ViewBag.OfferId = offer.OfferId;
        ViewBag.LotId = offer.ProduceLotId;

        var model = new CounterOfferViewModel
        {
            PricePerQuintal = offer.CurrentPricePerQuintal
        };

        return View(model);
    }

    // POST: /Offer/Counter/1
    [Authorize(Roles = "Farmer")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Counter(
        int id,
        CounterOfferViewModel model)
    {
        var userId = _userManager.GetUserId(User);

        if (userId == null)
            return Forbid();

        var farmer = await _context.FarmerProfiles
            .FirstOrDefaultAsync(f => f.UserId == userId);

        if (farmer == null)
            return Forbid();

        var offer = await _context.Offers
            .Include(o => o.ProduceLot)
                .ThenInclude(l => l.Crop)
            .Include(o => o.Revisions)
            .FirstOrDefaultAsync(o =>
                o.OfferId == id &&
                o.ProduceLot.FarmerProfileId ==
                farmer.FarmerProfileId);

        if (offer == null)
            return NotFound();

        if (offer.Status != OfferStatus.Pending)
        {
            TempData["ErrorMessage"] =
                "This offer can no longer be countered.";

            return RedirectToAction(nameof(Incoming));
        }

        if (offer.ProduceLot.Status != ProduceLotStatus.Published)
        {
            TempData["ErrorMessage"] =
                "This produce lot is no longer available.";

            return RedirectToAction(nameof(Incoming));
        }

        ViewBag.CropName = offer.ProduceLot.Crop.Name;
        ViewBag.OfferId = offer.OfferId;
        ViewBag.LotId = offer.ProduceLotId;

        if (!ModelState.IsValid)
            return View(model);

        var nextRevisionNumber = offer.Revisions
            .Select(r => r.RevisionNumber)
            .DefaultIfEmpty(0)
            .Max() + 1;

        var now = DateTime.UtcNow;

        var revision = new OfferRevision
        {
            OfferId = offer.OfferId,
            RevisionNumber = nextRevisionNumber,
            ProposedByUserId = userId,
            ProposedPricePerQuintal = model.PricePerQuintal,
            ProposedQuantityQuintals = offer.QuantityQuintals,
            TransportationCost = offer.TransportationCost,
            LoadingCost = offer.LoadingCost,
            OtherFarmerCost = offer.OtherFarmerCost,
            Message = string.IsNullOrWhiteSpace(model.Message)
                ? null
                : model.Message.Trim(),
            CreatedAtUtc = now
        };

        offer.CurrentPricePerQuintal = model.PricePerQuintal;
        offer.Message = revision.Message;
        offer.Status = OfferStatus.Countered;
        offer.UpdatedAtUtc = now;

        _context.OfferRevisions.Add(revision);

        try
        {
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Your counteroffer has been sent to the buyer.";
        }
        catch (DbUpdateException)
        {
            TempData["ErrorMessage"] =
                "The counteroffer could not be submitted. Please try again.";
        }

        return RedirectToAction(nameof(Incoming));
    }

    // Helper: get the currently logged-in buyer profile.
    private async Task<BuyerProfile?> GetCurrentBuyerProfileAsync()
    {
        var userId = _userManager.GetUserId(User);

        if (userId == null)
            return null;

        return await _context.BuyerProfiles
            .FirstOrDefaultAsync(p => p.UserId == userId);
    }

    // Helper: get a published produce lot.
    private async Task<ProduceLot?> GetPublishedLotAsync(int produceLotId)
    {
        return await _context.ProduceLots
            .Include(l => l.Crop)
            .FirstOrDefaultAsync(l =>
                l.ProduceLotId == produceLotId &&
                l.Status == ProduceLotStatus.Published);
    }
}
