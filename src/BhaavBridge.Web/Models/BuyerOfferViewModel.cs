
namespace BhaavBridge.Web.Models.ViewModels;

public sealed class BuyerOfferViewModel
{
    public int OfferId { get; set; }
    public int ProduceLotId { get; set; }

    public string CropName { get; set; } = string.Empty;
    public string FarmerName { get; set; } = string.Empty;

    public decimal QuantityQuintals { get; set; }

    public decimal OriginalPricePerQuintal { get; set; }
    public decimal CurrentPricePerQuintal { get; set; }

    public decimal TransportationCost { get; set; }
    public decimal LoadingCost { get; set; }
    public decimal OtherFarmerCost { get; set; }

    public decimal GrossValue { get; set; }
    public decimal TotalEstimatedCosts { get; set; }
    public decimal NetDaam { get; set; }

    public string? Message { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; }

    public List<BuyerOfferRevisionViewModel> Revisions { get; set; } = new();
}

public sealed class BuyerOfferRevisionViewModel
{
    public int RevisionNumber { get; set; }
    public string ProposedByName { get; set; } = string.Empty;

    public decimal ProposedPricePerQuintal { get; set; }
    public decimal ProposedQuantityQuintals { get; set; }

    public decimal TransportationCost { get; set; }
    public decimal LoadingCost { get; set; }
    public decimal OtherFarmerCost { get; set; }

    public string? Message { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}
