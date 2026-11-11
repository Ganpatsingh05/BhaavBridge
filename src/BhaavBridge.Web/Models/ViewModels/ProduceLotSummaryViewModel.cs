namespace BhaavBridge.Web.Models.ViewModels;

/// <summary>
/// Lightweight read model used in list / marketplace views.
/// </summary>
public sealed class ProduceLotSummaryViewModel
{
    public int ProduceLotId { get; set; }
    public string CropName { get; set; } = string.Empty;
    public string? Variety { get; set; }
    public string? QualityGrade { get; set; }
    public decimal QuantityQuintals { get; set; }
    public DateOnly HarvestDate { get; set; }
    public string District { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public decimal AskingPricePerQuintal { get; set; }
    public string? Description { get; set; }
    public string FarmerName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; }
}
