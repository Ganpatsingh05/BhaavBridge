using System.ComponentModel.DataAnnotations;

namespace BhaavBridge.Web.Models.Api;

public sealed class CreateOfferRequest
{
    [Range(1, int.MaxValue)]
    public int ProduceLotId { get; set; }

    [Range(typeof(decimal), "0.01", "9999999999999999.99")]
    public decimal PricePerQuintal { get; set; }

    [Range(typeof(decimal), "0", "9999999999999999.99")]
    public decimal TransportationCost { get; set; }

    [Range(typeof(decimal), "0", "9999999999999999.99")]
    public decimal LoadingCost { get; set; }

    [Range(typeof(decimal), "0", "9999999999999999.99")]
    public decimal OtherFarmerCost { get; set; }

    [StringLength(1000)]
    public string? Message { get; set; }
}
