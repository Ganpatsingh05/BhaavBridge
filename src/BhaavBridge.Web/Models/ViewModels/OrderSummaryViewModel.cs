using BhaavBridge.Web.Models.Entities;

namespace BhaavBridge.Web.Models.ViewModels;

public sealed class OrderSummaryViewModel
{
    public int OrderId { get; set; }
    public int OfferId { get; set; }
    public int ProduceLotId { get; set; }
    public string CropName { get; set; } = string.Empty;
    public string FarmerName { get; set; } = string.Empty;
    public string BuyerBusinessName { get; set; } = string.Empty;
    public decimal QuantityQuintals { get; set; }
    public decimal AgreedPricePerQuintal { get; set; }
    public decimal GrossValue { get; set; }
    public decimal TransportationCost { get; set; }
    public decimal LoadingCost { get; set; }
    public decimal OtherFarmerCost { get; set; }
    public decimal TotalFarmerCosts { get; set; }
    public decimal EstimatedNetReturn { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
}
