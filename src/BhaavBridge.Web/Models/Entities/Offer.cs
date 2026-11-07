namespace BhaavBridge.Web.Models.Entities;

public sealed class Offer : IAuditedEntity
{
    public int OfferId { get; set; }
    public int ProduceLotId { get; set; }
    public int BuyerProfileId { get; set; }
    public decimal CurrentPricePerQuintal { get; set; }
    public decimal QuantityQuintals { get; set; }
    public decimal TransportationCost { get; set; }
    public decimal LoadingCost { get; set; }
    public decimal OtherFarmerCost { get; set; }
    public string? Message { get; set; }
    public OfferStatus Status { get; set; } = OfferStatus.Pending;
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }

    public ProduceLot ProduceLot { get; set; } = null!;
    public BuyerProfile BuyerProfile { get; set; } = null!;
    public ICollection<OfferRevision> Revisions { get; set; } = new List<OfferRevision>();
    public Order? Order { get; set; }
}
