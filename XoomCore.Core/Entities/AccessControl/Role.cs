using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using XoomCore.Core.Enum;
using XoomCore.Core.Shared;

namespace XoomCore.Core.Entities.AccessControl;

// Unique index for the entity
[Index(nameof(Name), IsUnique = true)]
public class Role : AuditableEntity
{
    [Column(TypeName = "nvarchar(100)")]
    public string Name { get; set; }

    [Column(TypeName = "nvarchar(250)")]
    public string? Description { get; set; }
    public EntityStatus Status { get; set; } = EntityStatus.IsActive;


    // =================================================
    // Navigation property - associated with this entity
    // =================================================

    public virtual ICollection<RoleActionAuthorization> RoleActionAuthorizations { get; set; }
    public virtual ICollection<UserRole> UserRoles { get; set; }
}

