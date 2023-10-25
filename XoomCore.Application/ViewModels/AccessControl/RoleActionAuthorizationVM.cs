using XoomCore.Core.Enum;

namespace XoomCore.Application.ViewModels.AccessControl;

public class RoleActionAuthorizationVM
{
    public long Id { get; set; }
    public long RoleId { get; set; }
    public string RoleName { get; set; }
    public long ActionAuthorizationId { get; set; }
    public string ActionAuthorizationName { get; set; }
    public EntityStatus Status { get; set; }
}
