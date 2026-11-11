using System.ComponentModel.DataAnnotations;

namespace BhaavBridge.Web.Models.ViewModels;

/// <summary>Used by Admin to create or edit a crop in the catalogue.</summary>
public sealed class CropViewModel
{
    [Required(ErrorMessage = "Crop name is required.")]
    [StringLength(100, ErrorMessage = "Name must not exceed 100 characters.")]
    [Display(Name = "Crop Name")]
    public string Name { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "Category must not exceed 100 characters.")]
    public string? Category { get; set; }

    [Display(Name = "Active")]
    public bool IsActive { get; set; } = true;
}
