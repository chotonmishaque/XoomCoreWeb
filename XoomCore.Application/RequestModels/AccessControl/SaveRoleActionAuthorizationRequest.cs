namespace XoomCore.Application.RequestModels.AccessControl;

public class SaveRoleActionAuthorizationRequest
{
    public long Id { get; set; }
    public long RoleId { get; set; }
    public long ActionAuthorizationId { get; set; }
    public int Status { get; set; }
}
