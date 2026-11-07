namespace BhaavBridge.Web.Models.Entities;

public sealed class Order : IAuditedEntity
{
    public int OrderId { get; set; }
    public int OfferId { get; set; }
    public decimal AgreedQuantityQuintals { get; set; }
    public decimal AgreedPricePerQuintal { get; set; }
    public decimal GrossValue { get; set; }
    public decimal TransportationCostSnapshot { get; set; }
    public decimal LoadingCostSnapshot { get; set; }
    public decimal OtherFarmerCostSnapshot { get; set; }
    public decimal TotalFarmerCostsSnapshot { get; set; }
    public decimal EstimatedNetReturnSnapshot { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Placed;
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }

    public Offer Offer { get; set; } = null!;
}
