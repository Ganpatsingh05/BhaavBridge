namespace BhaavBridge.Web.Models.Entities;

public sealed class Crop
{
    public int CropId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Category { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<ProduceLot> ProduceLots { get; set; } = new List<ProduceLot>();
    public ICollection<MarketReferencePrice> MarketReferencePrices { get; set; } = new List<MarketReferencePrice>();
}
