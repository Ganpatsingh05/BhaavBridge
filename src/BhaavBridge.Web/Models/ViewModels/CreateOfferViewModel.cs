
using System.ComponentModel.DataAnnotations;

namespace BhaavBridge.Web.Models.ViewModels;

public sealed class CreateOfferViewModel
{
    public int ProduceLotId { get; set; }

    [Required(ErrorMessage = "Please enter your offer price.")]
    [Range(typeof(decimal), "0.01", "9999999999999999.99",
        ErrorMessage = "Offer price must be greater than zero.")]
    [Display(Name = "Offer Price per Quintal (\u20B9)")]
    public decimal PricePerQuintal { get; set; }

    [Range(typeof(decimal), "0", "9999999999999999.99",
        ErrorMessage = "Transportation cost cannot be negative.")]
    [Display(Name = "Estimated Transportation Cost (\u20B9)")]
    public decimal TransportationCost { get; set; }

    [Range(typeof(decimal), "0", "9999999999999999.99",
        ErrorMessage = "Loading cost cannot be negative.")]
    [Display(Name = "Estimated Loading Cost (\u20B9)")]
    public decimal LoadingCost { get; set; }

    [Range(typeof(decimal), "0", "9999999999999999.99",
        ErrorMessage = "Other costs cannot be negative.")]
    [Display(Name = "Other Estimated Farmer Costs (\u20B9)")]
    public decimal OtherFarmerCost { get; set; }

    [StringLength(1000, ErrorMessage = "Message cannot exceed 1000 characters.")]
    [Display(Name = "Message to Farmer")]
    public string? Message { get; set; }
}
