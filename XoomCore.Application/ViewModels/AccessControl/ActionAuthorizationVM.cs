using XoomCore.Core.Enum;

namespace XoomCore.Application.ViewModels.AccessControl;

public class ActionAuthorizationVM
{
    public long Id { get; set; }
    public long MenuId { get; set; }
    public string MenuName { get; set; }
    public long SubMenuId { get; set; }
    public string SubMenuName { get; set; }
    public string Name { get; set; }
    public string Controller { get; set; }
    public string ActionMethod { get; set; }
    public int IsPageLinked { get; set; }
    public string? Description { get; set; }
    public EntityStatus Status { get; set; } = EntityStatus.IsActive;
}
