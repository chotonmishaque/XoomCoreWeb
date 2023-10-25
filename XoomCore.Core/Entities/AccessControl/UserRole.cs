using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using XoomCore.Core.Enum;
using XoomCore.Core.Shared;

namespace XoomCore.Core.Entities.AccessControl;


// Unique index for the entity
[Index(nameof(UserId), nameof(RoleId), IsUnique = true)]
public class UserRole : AuditableEntity
{
    public long UserId { get; set; }
    public long RoleId { get; set; }
    public EntityStatus Status { get; set; } = EntityStatus.IsActive;

    // =================================================
    // Navigation property - associated with this entity
    // =================================================

    [ForeignKey(nameof(UserId))]
    public User User { get; set; }
    [ForeignKey(nameof(RoleId))]
    public Role Role { get; set; }
}
