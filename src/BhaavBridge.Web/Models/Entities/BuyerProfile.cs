namespace BhaavBridge.Web.Models.Entities;

public sealed class BuyerProfile
{
    public int BuyerProfileId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string BusinessName { get; set; } = string.Empty;
    public string ContactPerson { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;

    public ApplicationUser User { get; set; } = null!;
    public ICollection<Offer> Offers { get; set; } = new List<Offer>();
}
