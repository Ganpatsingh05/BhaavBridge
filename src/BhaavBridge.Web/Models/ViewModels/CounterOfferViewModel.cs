
using System.ComponentModel.DataAnnotations;

namespace BhaavBridge.Web.Models.ViewModels;

public sealed class CounterOfferViewModel
{
    [Required(ErrorMessage = "Please enter your counteroffer price.")]
    [Range(typeof(decimal), "0.01", "9999999999999999.99",
        ErrorMessage = "Counteroffer price must be greater than zero.")]
    [Display(Name = "Counteroffer Price per Quintal (₹)")]
    public decimal PricePerQuintal { get; set; }

    [StringLength(1000,
        ErrorMessage = "Message cannot exceed 1000 characters.")]
    [Display(Name = "Message to Buyer")]
    public string? Message { get; set; }
}
