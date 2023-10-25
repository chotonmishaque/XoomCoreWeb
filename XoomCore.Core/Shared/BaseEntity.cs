using System.ComponentModel.DataAnnotations;

namespace XoomCore.Core.Shared;

public abstract class BaseEntity
{
    // Unique identifier for the entity
    [Key]
    public long Id { get; set; }
}
