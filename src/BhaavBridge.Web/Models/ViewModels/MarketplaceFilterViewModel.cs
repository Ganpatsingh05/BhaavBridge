namespace BhaavBridge.Web.Models.ViewModels;

/// <summary>
/// Carries buyer-side marketplace search / filter parameters and the
/// paged result list.  All filter fields are optional.
/// </summary>
public sealed class MarketplaceFilterViewModel
{
    // --- filter inputs ---
    public int? CropId { get; set; }
    public string? District { get; set; }
    public string? State { get; set; }
    public decimal? MinQuantity { get; set; }
    public decimal? MaxQuantity { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }

    // --- results ---
    public IReadOnlyList<ProduceLotSummaryViewModel> Lots { get; set; }
        = Array.Empty<ProduceLotSummaryViewModel>();

    // --- drop-down data (populated by controller) ---
    public IEnumerable<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> CropSelectList { get; set; }
        = Enumerable.Empty<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
}
