using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BhaavBridge.Web.Models.ViewModels;

public sealed class MarketReferencePriceFormViewModel
{
    public int? MarketReferencePriceId { get; set; }

    [Required(ErrorMessage = "Select a crop.")]
    [Range(1, int.MaxValue, ErrorMessage = "Select a valid crop.")]
    [Display(Name = "Crop")]
    public int CropId { get; set; }

    [Required, StringLength(150)]
    [Display(Name = "Market name")]
    public string MarketName { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string District { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string State { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Effective date")]
    public DateOnly EffectiveDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    [Range(typeof(decimal), "0", "9999999999999999.99")]
    [Display(Name = "Minimum price (₹/quintal)")]
    public decimal? MinimumPrice { get; set; }

    [Range(typeof(decimal), "0", "9999999999999999.99")]
    [Display(Name = "Maximum price (₹/quintal)")]
    public decimal? MaximumPrice { get; set; }

    [Required]
    [Range(typeof(decimal), "0.01", "9999999999999999.99")]
    [Display(Name = "Modal price (₹/quintal)")]
    public decimal ModalPrice { get; set; }

    [Required, StringLength(200)]
    [Display(Name = "Source name")]
    public string SourceName { get; set; } = string.Empty;

    [Url, StringLength(500)]
    [Display(Name = "Source URL (optional)")]
    public string? SourceUrl { get; set; }

    public IEnumerable<SelectListItem> CropOptions { get; set; }
        = Enumerable.Empty<SelectListItem>();
}
