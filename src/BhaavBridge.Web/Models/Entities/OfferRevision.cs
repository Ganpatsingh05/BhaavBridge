namespace BhaavBridge.Web.Models.Entities;

public sealed class OfferRevision
{
    public int OfferRevisionId { get; set; }
    public int OfferId { get; set; }
    public int RevisionNumber { get; set; }
    public string ProposedByUserId { get; set; } = string.Empty;
    public decimal ProposedPricePerQuintal { get; set; }
    public decimal ProposedQuantityQuintals { get; set; }
    public decimal TransportationCost { get; set; }
    public decimal LoadingCost { get; set; }
    public decimal OtherFarmerCost { get; set; }
    public string? Message { get; set; }
    public DateTime CreatedAtUtc { get; set; }

    public Offer Offer { get; set; } = null!;
    public ApplicationUser ProposedByUser { get; set; } = null!;
}
