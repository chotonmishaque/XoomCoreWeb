using System.ComponentModel.DataAnnotations.Schema;
using XoomCore.Core.Entities.AccessControl;

namespace XoomCore.Core.Shared;

public abstract class AuditableEntity : BaseEntity, IAuditableEntity
{
    public long? CreatedBy { get; set; }
    public DateTime? CreatedAt { get; set; }
    public long? UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // =================================================
    // Navigation property - associated with this entity
    // =================================================

    [ForeignKey(nameof(CreatedBy))]
    public User CreatedByUser { get; set; }
    [ForeignKey(nameof(UpdatedBy))]
    public User UpdatedByUser { get; set; }
}
