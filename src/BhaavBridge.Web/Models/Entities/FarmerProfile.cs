namespace BhaavBridge.Web.Models.Entities;

public sealed class FarmerProfile
{
    public int FarmerProfileId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Village { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string? FarmName { get; set; }

    public ApplicationUser User { get; set; } = null!;
    public ICollection<ProduceLot> ProduceLots { get; set; } = new List<ProduceLot>();
}
