using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using XoomCore.Core.Enum;
using XoomCore.Core.Shared;

namespace XoomCore.Core.Entities.AccessControl;

// Unique index for the entity
[Index(nameof(SubMenuId), nameof(Name), IsUnique = true)]
[Index(nameof(SubMenuId), nameof(ActionMethod), IsUnique = true)]
public class ActionAuthorization : AuditableEntity
{
    public long SubMenuId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    [Required]
    [MaxLength(100)]
    public string Controller { get; set; }

    [Required]
    [MaxLength(100)]
    public string ActionMethod { get; set; }
    public int IsPageLinked { get; set; }

    [MaxLength(250)]
    public string? Description { get; set; }
    public EntityStatus Status { get; set; } = EntityStatus.IsActive;

    // =================================================
    // Navigation property - associated with this entity
    // =================================================

    [ForeignKey(nameof(SubMenuId))]
    public virtual SubMenu SubMenu { get; set; }
    public virtual ICollection<RoleActionAuthorization> RoleActionPermissions { get; set; }
}
