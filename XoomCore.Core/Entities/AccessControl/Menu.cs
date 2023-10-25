using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using XoomCore.Core.Enum;
using XoomCore.Core.Shared;

namespace XoomCore.Core.Entities.AccessControl;

// Unique index for the entity
[Index(nameof(Name), IsUnique = true)]
public class Menu : AuditableEntity
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    [MaxLength(250)]
    public string? Description { get; set; }

    public int DisplaySequence { get; set; }

    [MaxLength(50)]
    public string? Icon { get; set; } = "bx bx-layout";

    public EntityStatus Status { get; set; } = EntityStatus.IsActive;
    // Other relevant properties for your specific use case


    // =================================================
    // Navigation property - associated with this entity
    // =================================================
    public virtual ICollection<SubMenu> SubMenus { get; set; }

}