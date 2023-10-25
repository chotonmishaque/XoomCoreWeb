using System.ComponentModel.DataAnnotations.Schema;
using XoomCore.Core.Entities.AccessControl;

namespace XoomCore.Core.Shared;

public abstract class SoftDeletableEntity : AuditableEntity, ISoftDelete
{
    public long? DeletedBy { get; set; }
    public DateTime? DeletedAt { get; set; }

    // =================================================
    // Navigation property - associated with this entity
    // =================================================

    [ForeignKey(nameof(DeletedBy))]
    public User DeletedByUser { get; set; }
}
