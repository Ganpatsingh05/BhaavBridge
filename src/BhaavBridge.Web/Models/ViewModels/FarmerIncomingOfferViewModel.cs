namespace BhaavBridge.Web.Models.ViewModels;

public sealed class FarmerIncomingOfferViewModel
{
    public int OfferId { get; set; }

    public int ProduceLotId { get; set; }

    public string CropName { get; set; } = string.Empty;

    public string BuyerBusinessName { get; set; } = string.Empty;

    public string BuyerContactPerson { get; set; } = string.Empty;

    public string BuyerFullName { get; set; } = string.Empty;

    public decimal QuantityQuintals { get; set; }

    public decimal OfferPricePerQuintal { get; set; }

    public decimal TransportationCost { get; set; }

    public decimal LoadingCost { get; set; }

    public decimal OtherFarmerCost { get; set; }

    public decimal GrossValue { get; set; }

    public decimal TotalEstimatedCosts { get; set; }

    public decimal NetDaam { get; set; }

    public decimal NetDaamPerQuintal { get; set; }

    public string? Message { get; set; }

    public string Status { get; set; } = string.Empty;

    public DateTime CreatedAtUtc { get; set; }
}
