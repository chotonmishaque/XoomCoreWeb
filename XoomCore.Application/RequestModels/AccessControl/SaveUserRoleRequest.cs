namespace XoomCore.Application.RequestModels.AccessControl;

public class SaveUserRoleRequest
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public long RoleId { get; set; }
    public int Status { get; set; }
}
