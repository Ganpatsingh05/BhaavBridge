using System.ComponentModel.DataAnnotations;

namespace BhaavBridge.Web.Models.ViewModels;

/// <summary>
/// Used by Farmers to create or edit a ProduceLot.
/// CropSelectList is populated by the controller and not bound from the form.
/// </summary>
public sealed class ProduceLotFormViewModel
{
    [Required(ErrorMessage = "Please select a crop.")]
    [Display(Name = "Crop")]
    public int CropId { get; set; }

    [StringLength(100, ErrorMessage = "Variety must not exceed 100 characters.")]
    public string? Variety { get; set; }

    [StringLength(50, ErrorMessage = "Quality grade must not exceed 50 characters.")]
    [Display(Name = "Quality Grade")]
    public string? QualityGrade { get; set; }

    [Required(ErrorMessage = "Quantity is required.")]
    [Range(0.001, double.MaxValue, ErrorMessage = "Quantity must be greater than zero.")]
    [Display(Name = "Quantity (quintals)")]
    public decimal QuantityQuintals { get; set; }

    [Required(ErrorMessage = "Harvest date is required.")]
    [Display(Name = "Harvest Date")]
    public DateOnly HarvestDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    [Required(ErrorMessage = "District is required.")]
    [StringLength(100, ErrorMessage = "District must not exceed 100 characters.")]
    public string District { get; set; } = string.Empty;

    [Required(ErrorMessage = "State is required.")]
    [StringLength(100, ErrorMessage = "State must not exceed 100 characters.")]
    public string State { get; set; } = string.Empty;

    [Required(ErrorMessage = "Asking price is required.")]
    [Range(0, double.MaxValue, ErrorMessage = "Asking price must be zero or greater.")]
    [Display(Name = "Asking Price (₹ per quintal)")]
    public decimal AskingPricePerQuintal { get; set; }

    [StringLength(1000, ErrorMessage = "Description must not exceed 1000 characters.")]
    public string? Description { get; set; }

    // Populated by controller; not bound from form POST.
    public IEnumerable<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> CropSelectList { get; set; }
        = Enumerable.Empty<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
}
