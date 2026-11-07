using BhaavBridge.Web.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BhaavBridge.Web.Data;

public sealed class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<FarmerProfile> FarmerProfiles => Set<FarmerProfile>();
    public DbSet<BuyerProfile> BuyerProfiles => Set<BuyerProfile>();
    public DbSet<Crop> Crops => Set<Crop>();
    public DbSet<ProduceLot> ProduceLots => Set<ProduceLot>();
    public DbSet<MarketReferencePrice> MarketReferencePrices => Set<MarketReferencePrice>();
    public DbSet<Offer> Offers => Set<Offer>();
    public DbSet<OfferRevision> OfferRevisions => Set<OfferRevision>();
    public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.Property(x => x.FullName).HasMaxLength(150).IsRequired();
        });

        modelBuilder.Entity<FarmerProfile>(entity =>
        {
            entity.ToTable("FarmerProfiles");
            entity.HasKey(x => x.FarmerProfileId);
            entity.Property(x => x.UserId).HasMaxLength(450).IsRequired();
            entity.Property(x => x.Village).HasMaxLength(100).IsRequired();
            entity.Property(x => x.District).HasMaxLength(100).IsRequired();
            entity.Property(x => x.State).HasMaxLength(100).IsRequired();
            entity.Property(x => x.FarmName).HasMaxLength(150);

            entity.HasIndex(x => x.UserId).IsUnique();
            entity.HasOne(x => x.User)
                .WithOne()
                .HasForeignKey<FarmerProfile>(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<BuyerProfile>(entity =>
        {
            entity.ToTable("BuyerProfiles");
            entity.HasKey(x => x.BuyerProfileId);
            entity.Property(x => x.UserId).HasMaxLength(450).IsRequired();
            entity.Property(x => x.BusinessName).HasMaxLength(150).IsRequired();
            entity.Property(x => x.ContactPerson).HasMaxLength(150).IsRequired();
            entity.Property(x => x.Address).HasMaxLength(300).IsRequired();
            entity.Property(x => x.District).HasMaxLength(100).IsRequired();
            entity.Property(x => x.State).HasMaxLength(100).IsRequired();

            entity.HasIndex(x => x.UserId).IsUnique();
            entity.HasOne(x => x.User)
                .WithOne()
                .HasForeignKey<BuyerProfile>(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Crop>(entity =>
        {
            entity.ToTable("Crops");
            entity.HasKey(x => x.CropId);
            entity.Property(x => x.Name).HasMaxLength(100).IsRequired();
            entity.Property(x => x.Category).HasMaxLength(100);
            entity.Property(x => x.IsActive).HasDefaultValue(true);
            entity.HasIndex(x => x.Name).IsUnique();
        });

        modelBuilder.Entity<ProduceLot>(entity =>
        {
            entity.ToTable("ProduceLots", table =>
            {
                table.HasCheckConstraint("CK_ProduceLots_Quantity_Positive", "[QuantityQuintals] > 0");
                table.HasCheckConstraint("CK_ProduceLots_AskingPrice_NonNegative", "[AskingPricePerQuintal] >= 0");
            });

            entity.HasKey(x => x.ProduceLotId);
            entity.Property(x => x.Variety).HasMaxLength(100);
            entity.Property(x => x.QualityGrade).HasMaxLength(50);
            entity.Property(x => x.QuantityQuintals).HasPrecision(12, 3);
            entity.Property(x => x.District).HasMaxLength(100).IsRequired();
            entity.Property(x => x.State).HasMaxLength(100).IsRequired();
            entity.Property(x => x.AskingPricePerQuintal).HasPrecision(18, 2);
            entity.Property(x => x.Description).HasMaxLength(1000);
            entity.Property(x => x.Status).HasConversion<string>().HasMaxLength(24);
            entity.Property(x => x.CreatedAtUtc).IsRequired();
            entity.Property(x => x.UpdatedAtUtc).IsRequired();

            entity.HasIndex(x => new { x.Status, x.CropId });
            entity.HasIndex(x => new { x.District, x.State });
            entity.HasOne(x => x.FarmerProfile)
                .WithMany(x => x.ProduceLots)
                .HasForeignKey(x => x.FarmerProfileId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(x => x.Crop)
                .WithMany(x => x.ProduceLots)
                .HasForeignKey(x => x.CropId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<MarketReferencePrice>(entity =>
        {
            entity.ToTable("MarketReferencePrices", table =>
            {
                table.HasCheckConstraint(
                    "CK_MarketReferencePrices_Prices_NonNegative",
                    "([MinimumPrice] IS NULL OR [MinimumPrice] >= 0) AND " +
                    "([MaximumPrice] IS NULL OR [MaximumPrice] >= 0) AND [ModalPrice] >= 0");
                table.HasCheckConstraint(
                    "CK_MarketReferencePrices_MinMax_Order",
                    "[MinimumPrice] IS NULL OR [MaximumPrice] IS NULL OR [MinimumPrice] <= [MaximumPrice]");
            });

            entity.HasKey(x => x.MarketReferencePriceId);
            entity.Property(x => x.MarketName).HasMaxLength(150).IsRequired();
            entity.Property(x => x.District).HasMaxLength(100).IsRequired();
            entity.Property(x => x.State).HasMaxLength(100).IsRequired();
            entity.Property(x => x.MinimumPrice).HasPrecision(18, 2);
            entity.Property(x => x.MaximumPrice).HasPrecision(18, 2);
            entity.Property(x => x.ModalPrice).HasPrecision(18, 2);
            entity.Property(x => x.SourceName).HasMaxLength(200).IsRequired();
            entity.Property(x => x.SourceUrl).HasMaxLength(500);
            entity.Property(x => x.RecordedAtUtc).IsRequired();

            entity.HasIndex(x => new { x.CropId, x.MarketName, x.EffectiveDate });
            entity.HasOne(x => x.Crop)
                .WithMany(x => x.MarketReferencePrices)
                .HasForeignKey(x => x.CropId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Offer>(entity =>
        {
            entity.ToTable("Offers", table =>
            {
                table.HasCheckConstraint("CK_Offers_Price_NonNegative", "[CurrentPricePerQuintal] >= 0");
                table.HasCheckConstraint("CK_Offers_Quantity_Positive", "[QuantityQuintals] > 0");
                table.HasCheckConstraint(
                    "CK_Offers_FarmerCosts_NonNegative",
                    "[TransportationCost] >= 0 AND [LoadingCost] >= 0 AND [OtherFarmerCost] >= 0");
            });

            entity.HasKey(x => x.OfferId);
            entity.Property(x => x.CurrentPricePerQuintal).HasPrecision(18, 2);
            entity.Property(x => x.QuantityQuintals).HasPrecision(12, 3);
            entity.Property(x => x.TransportationCost).HasPrecision(18, 2);
            entity.Property(x => x.LoadingCost).HasPrecision(18, 2);
            entity.Property(x => x.OtherFarmerCost).HasPrecision(18, 2);
            entity.Property(x => x.Message).HasMaxLength(1000);
            entity.Property(x => x.Status).HasConversion<string>().HasMaxLength(24);
            entity.Property(x => x.CreatedAtUtc).IsRequired();
            entity.Property(x => x.UpdatedAtUtc).IsRequired();

            entity.HasIndex(x => new { x.ProduceLotId, x.BuyerProfileId })
                .IsUnique()
                .HasFilter("[Status] IN ('Pending', 'Countered')");
            entity.HasIndex(x => x.ProduceLotId)
                .IsUnique()
                .HasFilter("[Status] = 'Accepted'");
            entity.HasIndex(x => new { x.BuyerProfileId, x.Status });

            entity.HasOne(x => x.ProduceLot)
                .WithMany(x => x.Offers)
                .HasForeignKey(x => x.ProduceLotId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(x => x.BuyerProfile)
                .WithMany(x => x.Offers)
                .HasForeignKey(x => x.BuyerProfileId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<OfferRevision>(entity =>
        {
            entity.ToTable("OfferRevisions", table =>
            {
                table.HasCheckConstraint("CK_OfferRevisions_Price_NonNegative", "[ProposedPricePerQuintal] >= 0");
                table.HasCheckConstraint("CK_OfferRevisions_Quantity_Positive", "[ProposedQuantityQuintals] > 0");
                table.HasCheckConstraint(
                    "CK_OfferRevisions_FarmerCosts_NonNegative",
                    "[TransportationCost] >= 0 AND [LoadingCost] >= 0 AND [OtherFarmerCost] >= 0");
            });

            entity.HasKey(x => x.OfferRevisionId);
            entity.Property(x => x.ProposedByUserId).HasMaxLength(450).IsRequired();
            entity.Property(x => x.ProposedPricePerQuintal).HasPrecision(18, 2);
            entity.Property(x => x.ProposedQuantityQuintals).HasPrecision(12, 3);
            entity.Property(x => x.TransportationCost).HasPrecision(18, 2);
            entity.Property(x => x.LoadingCost).HasPrecision(18, 2);
            entity.Property(x => x.OtherFarmerCost).HasPrecision(18, 2);
            entity.Property(x => x.Message).HasMaxLength(1000);
            entity.Property(x => x.CreatedAtUtc).IsRequired();

            entity.HasIndex(x => new { x.OfferId, x.RevisionNumber }).IsUnique();
            entity.HasOne(x => x.Offer)
                .WithMany(x => x.Revisions)
                .HasForeignKey(x => x.OfferId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(x => x.ProposedByUser)
                .WithMany()
                .HasForeignKey(x => x.ProposedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Orders", table =>
            {
                table.HasCheckConstraint("CK_Orders_Quantity_Positive", "[AgreedQuantityQuintals] > 0");
                table.HasCheckConstraint(
                    "CK_Orders_NonNegativeAmounts",
                    "[AgreedPricePerQuintal] >= 0 AND [GrossValue] >= 0 AND " +
                    "[TransportationCostSnapshot] >= 0 AND [LoadingCostSnapshot] >= 0 AND " +
                    "[OtherFarmerCostSnapshot] >= 0 AND [TotalFarmerCostsSnapshot] >= 0");
            });

            entity.HasKey(x => x.OrderId);
            entity.Property(x => x.AgreedQuantityQuintals).HasPrecision(12, 3);
            entity.Property(x => x.AgreedPricePerQuintal).HasPrecision(18, 2);
            entity.Property(x => x.GrossValue).HasPrecision(18, 2);
            entity.Property(x => x.TransportationCostSnapshot).HasPrecision(18, 2);
            entity.Property(x => x.LoadingCostSnapshot).HasPrecision(18, 2);
            entity.Property(x => x.OtherFarmerCostSnapshot).HasPrecision(18, 2);
            entity.Property(x => x.TotalFarmerCostsSnapshot).HasPrecision(18, 2);
            entity.Property(x => x.EstimatedNetReturnSnapshot).HasPrecision(18, 2);
            entity.Property(x => x.Status).HasConversion<string>().HasMaxLength(24);
            entity.Property(x => x.CreatedAtUtc).IsRequired();
            entity.Property(x => x.UpdatedAtUtc).IsRequired();

            entity.HasIndex(x => x.OfferId).IsUnique();
            entity.HasOne(x => x.Offer)
                .WithOne(x => x.Order)
                .HasForeignKey<Order>(x => x.OfferId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    public override int SaveChanges()
    {
        return SaveChanges(acceptAllChangesOnSuccess: true);
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        ApplyAuditTimestamps();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return SaveChangesAsync(acceptAllChangesOnSuccess: true, cancellationToken: cancellationToken);
    }

    public override Task<int> SaveChangesAsync(
        bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default)
    {
        ApplyAuditTimestamps();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void ApplyAuditTimestamps()
    {
        var now = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<IAuditedEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAtUtc = now;
                entry.Entity.UpdatedAtUtc = now;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAtUtc = now;
            }
        }
    }
}
