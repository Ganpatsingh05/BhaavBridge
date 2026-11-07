namespace BhaavBridge.Web.Models.Entities;

public interface IAuditedEntity
{
    DateTime CreatedAtUtc { get; set; }
    DateTime UpdatedAtUtc { get; set; }
}
