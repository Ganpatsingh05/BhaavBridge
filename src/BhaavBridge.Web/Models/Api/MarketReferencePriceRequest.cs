using System.ComponentModel.DataAnnotations;

namespace BhaavBridge.Web.Models.Api;

public sealed class MarketReferencePriceRequest
{
    [Range(1, int.MaxValue)]
    public int CropId { get; set; }

    [Required, StringLength(150)]
    public string MarketName { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string District { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string State { get; set; } = string.Empty;

    public DateOnly EffectiveDate { get; set; }

    [Range(typeof(decimal), "0", "9999999999999999.99")]
    public decimal? MinimumPrice { get; set; }

    [Range(typeof(decimal), "0", "9999999999999999.99")]
    public decimal? MaximumPrice { get; set; }

    [Range(typeof(decimal), "0.01", "9999999999999999.99")]
    public decimal ModalPrice { get; set; }

    [Required, StringLength(200)]
    public string SourceName { get; set; } = string.Empty;

    [Url, StringLength(500)]
    public string? SourceUrl { get; set; }
}

public sealed class UpdateOrderStatusRequest
{
    public BhaavBridge.Web.Models.Entities.OrderStatus Status { get; set; }
}
