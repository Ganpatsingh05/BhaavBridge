using System.ComponentModel.DataAnnotations;

namespace BhaavBridge.Web.Models.Api;

public sealed class CounterOfferRequest
{
    [Range(typeof(decimal), "0.01", "9999999999999999.99")]
    public decimal PricePerQuintal { get; set; }

    [StringLength(1000)]
    public string? Message { get; set; }
}
