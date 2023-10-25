using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using XoomCore.Core.Entities.AccessControl;

namespace XoomCore.Core.Entities.Report;

[Table("_EntityLogs")]
public class EntityLog
{
    [Key]
    public long Id { get; set; }
    public string EntityName { get; set; }
    public string ActionType { get; set; }
    public string PrimaryRefId { get; set; }
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }
    public string? AffectedColumn { get; set; }
    public long? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }

    // =================================================
    // Navigation property - associated with this entity
    // =================================================

    [ForeignKey(nameof(CreatedBy))]
    public User CreatedByUser { get; set; }
}
