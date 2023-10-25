using XoomCore.Core.Enum;

namespace XoomCore.Application.ViewModels.AccessControl;

public class RoleVM
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public EntityStatus Status { get; set; }
}
