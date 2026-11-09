using System.ComponentModel.DataAnnotations;

namespace BhaavBridge.Web.Models.ViewModels;

public sealed class BuyerProfileViewModel
{
    [Required]
    [StringLength(150)]
    [Display(Name = "Full Name")]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [StringLength(150)]
    [Display(Name = "Business Name")]
    public string BusinessName { get; set; } = string.Empty;

    [Required]
    [StringLength(150)]
    [Display(Name = "Contact Person")]
    public string ContactPerson { get; set; } = string.Empty;

    [Required]
    [StringLength(300)]
    public string Address { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string District { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string State { get; set; } = string.Empty;
}