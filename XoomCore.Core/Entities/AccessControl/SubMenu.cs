using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using XoomCore.Core.Enum;
using XoomCore.Core.Shared;

namespace XoomCore.Core.Entities.AccessControl;

// Unique index for the entity
[Index(nameof(MenuId), nameof(Name), IsUnique = true)]
public class SubMenu : AuditableEntity
{
    public long MenuId { get; set; }
    [Required]
    [MaxLength(50)]
    public string Key { get; set; } = "";
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }
    [Required]
    [MaxLength(250)]
    public string Url { get; set; }
    public int DisplaySequence { get; set; }
    public EntityStatus Status { get; set; } = EntityStatus.IsActive;


    // =================================================
    // Navigation property - associated with this entity
    // =================================================
    [ForeignKey(nameof(MenuId))]
    public Menu Menu { get; set; }
}