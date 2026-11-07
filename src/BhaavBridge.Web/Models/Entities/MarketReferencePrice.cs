namespace BhaavBridge.Web.Models.Entities;

public sealed class MarketReferencePrice
{
    public int MarketReferencePriceId { get; set; }
    public int CropId { get; set; }
    public string MarketName { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public DateOnly EffectiveDate { get; set; }
    public decimal? MinimumPrice { get; set; }
    public decimal? MaximumPrice { get; set; }
    public decimal ModalPrice { get; set; }
    public string SourceName { get; set; } = string.Empty;
    public string? SourceUrl { get; set; }
    public DateTime RecordedAtUtc { get; set; }

    public Crop Crop { get; set; } = null!;
}
