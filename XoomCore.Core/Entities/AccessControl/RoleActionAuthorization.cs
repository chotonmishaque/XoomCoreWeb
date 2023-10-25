using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using XoomCore.Core.Enum;
using XoomCore.Core.Shared;

namespace XoomCore.Core.Entities.AccessControl;

// Unique index for the entity
[Index(nameof(RoleId), nameof(ActionAuthorizationId), IsUnique = true)]
public class RoleActionAuthorization : AuditableEntity
{
    // Foreign keys
    public long RoleId { get; set; }
    public long ActionAuthorizationId { get; set; }
    public EntityStatus Status { get; set; } = EntityStatus.IsActive;


    // =================================================
    // Navigation property - associated with this entity
    // =================================================
    [ForeignKey(nameof(RoleId))]
    public virtual Role Role { get; set; }
    [ForeignKey(nameof(ActionAuthorizationId))]
    public virtual ActionAuthorization ActionAuthorization { get; set; }
}
