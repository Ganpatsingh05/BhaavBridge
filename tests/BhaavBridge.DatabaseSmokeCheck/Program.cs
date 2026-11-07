using BhaavBridge.Web.Data;
using BhaavBridge.Web.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

try
{
    var configuration = new ConfigurationBuilder()
        .AddUserSecrets<Program>()
        .Build();

    var connectionString = configuration.GetConnectionString("DefaultConnection");
    if (string.IsNullOrWhiteSpace(connectionString))
    {
        throw new InvalidOperationException(
            "DefaultConnection was not found in User Secrets. Configure it for the BhaavBridge-Web user-secrets ID.");
    }

    var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseSqlServer(connectionString)
        .Options;

    var token = Guid.NewGuid().ToString("N")[..10];
    var today = DateOnly.FromDateTime(DateTime.Today);

    await using var db = new ApplicationDbContext(options);
    await using var transaction = await db.Database.BeginTransactionAsync();

    Console.WriteLine("Connected to the configured SQL Server database.");

    var farmerUser = CreateUser($"smoke-farmer-{token}", $"farmer-{token}@example.test");
    var buyerUser = CreateUser($"smoke-buyer-{token}", $"buyer-{token}@example.test");

    db.Users.AddRange(farmerUser, buyerUser);
    await db.SaveChangesAsync();

    var farmer = new FarmerProfile
    {
        UserId = farmerUser.Id,
        Village = "Smoke Test Village",
        District = "Ludhiana",
        State = "Punjab",
        FarmName = "Temporary Smoke Test Farm"
    };

    var buyer = new BuyerProfile
    {
        UserId = buyerUser.Id,
        BusinessName = "Temporary Smoke Test Buyer",
        ContactPerson = "Smoke Test Contact",
        Address = "Temporary test address",
        District = "Ludhiana",
        State = "Punjab"
    };

    var crop = new Crop
    {
        Name = $"Smoke Test Crop {token}",
        Category = "Test",
        IsActive = true
    };

    db.AddRange(farmer, buyer, crop);
    await db.SaveChangesAsync();

    var lot = new ProduceLot
    {
        FarmerProfileId = farmer.FarmerProfileId,
        CropId = crop.CropId,
        Variety = "Smoke Test Variety",
        QualityGrade = "A",
        QuantityQuintals = 25m,
        HarvestDate = today,
        District = "Ludhiana",
        State = "Punjab",
        AskingPricePerQuintal = 2400m,
        Description = "Temporary record created by the database smoke check.",
        Status = ProduceLotStatus.Published
    };

    var referencePrice = new MarketReferencePrice
    {
        CropId = crop.CropId,
        MarketName = "Smoke Test Market",
        District = "Ludhiana",
        State = "Punjab",
        EffectiveDate = today,
        MinimumPrice = 2200m,
        MaximumPrice = 2500m,
        ModalPrice = 2400m,
        SourceName = "BhaavBridge database smoke check",
        RecordedAtUtc = DateTime.UtcNow
    };

    db.AddRange(lot, referencePrice);
    await db.SaveChangesAsync();

    var offer = new Offer
    {
        ProduceLotId = lot.ProduceLotId,
        BuyerProfileId = buyer.BuyerProfileId,
        CurrentPricePerQuintal = 2450m,
        QuantityQuintals = lot.QuantityQuintals,
        TransportationCost = 1500m,
        LoadingCost = 500m,
        OtherFarmerCost = 250m,
        Message = "Temporary smoke-check offer.",
        Status = OfferStatus.Pending
    };

    db.Offers.Add(offer);
    await db.SaveChangesAsync();

    db.OfferRevisions.Add(new OfferRevision
    {
        OfferId = offer.OfferId,
        RevisionNumber = 1,
        ProposedByUserId = buyerUser.Id,
        ProposedPricePerQuintal = offer.CurrentPricePerQuintal,
        ProposedQuantityQuintals = offer.QuantityQuintals,
        TransportationCost = offer.TransportationCost,
        LoadingCost = offer.LoadingCost,
        OtherFarmerCost = offer.OtherFarmerCost,
        Message = "Initial buyer offer.",
        CreatedAtUtc = DateTime.UtcNow
    });

    await db.SaveChangesAsync();

    offer.Status = OfferStatus.Accepted;
    lot.Status = ProduceLotStatus.Closed;

    var grossValue = offer.CurrentPricePerQuintal * offer.QuantityQuintals;
    var totalFarmerCosts = offer.TransportationCost + offer.LoadingCost + offer.OtherFarmerCost;

    db.Orders.Add(new Order
    {
        OfferId = offer.OfferId,
        AgreedQuantityQuintals = offer.QuantityQuintals,
        AgreedPricePerQuintal = offer.CurrentPricePerQuintal,
        GrossValue = grossValue,
        TransportationCostSnapshot = offer.TransportationCost,
        LoadingCostSnapshot = offer.LoadingCost,
        OtherFarmerCostSnapshot = offer.OtherFarmerCost,
        TotalFarmerCostsSnapshot = totalFarmerCosts,
        EstimatedNetReturnSnapshot = grossValue - totalFarmerCosts,
        Status = OrderStatus.Placed
    });

    await db.SaveChangesAsync();

    var loadedOrder = await db.Orders
        .Include(x => x.Offer)
            .ThenInclude(x => x.ProduceLot)
                .ThenInclude(x => x.Crop)
        .Include(x => x.Offer)
            .ThenInclude(x => x.Revisions)
        .SingleAsync(x => x.OfferId == offer.OfferId);

    if (loadedOrder.Offer.ProduceLot.Crop.Name != crop.Name ||
        loadedOrder.Offer.Revisions.Count != 1 ||
        loadedOrder.EstimatedNetReturnSnapshot != 59_000m)
    {
        throw new InvalidOperationException("The sample CRUD/read-back verification returned unexpected data.");
    }

    Console.WriteLine("PASS: Farmer, buyer, crop, harvest lot and market reference price inserted.");
    Console.WriteLine("PASS: Offer and OfferRevision inserted and read back.");
    Console.WriteLine("PASS: Accepted offer and Order inserted and read back.");
    Console.WriteLine($"PASS: Gross value = ₹{grossValue:N2}; farmer costs = ₹{totalFarmerCosts:N2}; estimated net return = ₹{loadedOrder.EstimatedNetReturnSnapshot:N2}.");

    db.ProduceLots.Add(new ProduceLot
    {
        FarmerProfileId = farmer.FarmerProfileId,
        CropId = crop.CropId,
        QuantityQuintals = -1m,
        HarvestDate = today,
        District = "Ludhiana",
        State = "Punjab",
        AskingPricePerQuintal = 100m,
        Status = ProduceLotStatus.Draft
    });

    try
    {
        await db.SaveChangesAsync();
        throw new InvalidOperationException("The database accepted a lot with a negative quantity; the check constraint did not behave as expected.");
    }
    catch (DbUpdateException exception) when (
        exception.GetBaseException().Message.Contains(
            "CK_ProduceLots_Quantity_Positive",
            StringComparison.OrdinalIgnoreCase))
    {
        Console.WriteLine("PASS: SQL Server rejected a negative harvest-lot quantity.");
    }

    await transaction.RollbackAsync();
    Console.WriteLine("PASS: Test transaction rolled back; no smoke-test rows were retained.");
    Console.WriteLine("DATABASE SMOKE CHECK PASSED.");
}
catch (Exception exception)
{
    Console.Error.WriteLine("DATABASE SMOKE CHECK FAILED.");
    Console.Error.WriteLine(exception);
    Environment.ExitCode = 1;
}

static ApplicationUser CreateUser(string userName, string email)
{
    return new ApplicationUser
    {
        Id = Guid.NewGuid().ToString(),
        UserName = userName,
        NormalizedUserName = userName.ToUpperInvariant(),
        Email = email,
        NormalizedEmail = email.ToUpperInvariant(),
        EmailConfirmed = true,
        FullName = "Temporary Smoke Test User"
    };
}
