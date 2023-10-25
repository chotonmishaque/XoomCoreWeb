using XoomCore.Application.ViewModels;

namespace XoomCore.Services.SessionControl;

public class SessionData
{
    public long UserId { get; set; }
    public List<SubMenuAuthorizedVM> SubMenuAuthorizedList { get; set; }
    public List<ActionAuthorizedVM> ActionAuthorizedList { get; set; }
}
public enum CoreController
{
    Home,
    MenuCategory,
    MenuItem,
    Role,
    RolePermission,
    User,
    UserRole
}

public enum CoreAction
{
    View,
    Create,
    Update,
    Delete
}
