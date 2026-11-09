using System.ComponentModel.DataAnnotations;

namespace BhaavBridge.Web.Models.ViewModels;

public sealed class FarmerProfileViewModel
{
    [Required]
    [StringLength(150)]
    [Display(Name = "Full Name")]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Village { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string District { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string State { get; set; } = string.Empty;

    [StringLength(150)]
    [Display(Name = "Farm Name (Optional)")]
    public string? FarmName { get; set; }
}