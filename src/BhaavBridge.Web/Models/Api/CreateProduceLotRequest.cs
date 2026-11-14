using System.ComponentModel.DataAnnotations;

namespace BhaavBridge.Web.Models.Api;

public sealed class CreateProduceLotRequest
{
    [Range(1, int.MaxValue)]
    public int CropId { get; set; }

    [StringLength(100)]
    public string? Variety { get; set; }

    [StringLength(50)]
    public string? QualityGrade { get; set; }

    [Range(typeof(decimal), "0.001", "999999999.999")]
    public decimal QuantityQuintals { get; set; }

    public DateOnly HarvestDate { get; set; }

    [Required, StringLength(100)]
    public string District { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string State { get; set; } = string.Empty;

    [Range(typeof(decimal), "0", "9999999999999999.99")]
    public decimal AskingPricePerQuintal { get; set; }

    [StringLength(1000)]
    public string? Description { get; set; }
}
