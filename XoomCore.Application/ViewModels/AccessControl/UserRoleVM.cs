using XoomCore.Core.Enum;

namespace XoomCore.Application.ViewModels.AccessControl;

public class UserRoleVM
{

    public long Id { get; set; }
    public long UserId { get; set; }
    public string Username { get; set; }
    public long RoleId { get; set; }
    public string RoleName { get; set; }
    public EntityStatus Status { get; set; }
}
