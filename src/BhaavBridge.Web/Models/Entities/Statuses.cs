namespace BhaavBridge.Web.Models.Entities;

public enum ProduceLotStatus
{
    Draft = 1,
    Published = 2,
    Closed = 3
}

public enum OfferStatus
{
    Pending = 1,
    Countered = 2,
    Accepted = 3,
    Rejected = 4,
    Withdrawn = 5
}

public enum OrderStatus
{
    Placed = 1,
    Confirmed = 2,
    ReadyForPickup = 3,
    Completed = 4,
    Cancelled = 5
}
