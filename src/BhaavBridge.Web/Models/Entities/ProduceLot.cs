namespace BhaavBridge.Web.Models.Entities;

public sealed class ProduceLot : IAuditedEntity
{
    public int ProduceLotId { get; set; }
    public int FarmerProfileId { get; set; }
    public int CropId { get; set; }
    public string? Variety { get; set; }
    public string? QualityGrade { get; set; }
    public decimal QuantityQuintals { get; set; }
    public DateOnly HarvestDate { get; set; }
    public string District { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public decimal AskingPricePerQuintal { get; set; }
    public string? Description { get; set; }
    public ProduceLotStatus Status { get; set; } = ProduceLotStatus.Draft;
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }

    public FarmerProfile FarmerProfile { get; set; } = null!;
    public Crop Crop { get; set; } = null!;
    public ICollection<Offer> Offers { get; set; } = new List<Offer>();
}
