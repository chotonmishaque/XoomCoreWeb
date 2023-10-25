using XoomCore.Core.Enum;

namespace XoomCore.Application.ViewModels.AccessControl;

public class SubMenuVM
{
    public long Id { get; set; }
    public long MenuId { get; set; }
    public string MenuName { get; set; }
    public string Key { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
    public int DisplaySequence { get; set; }
    public EntityStatus Status { get; set; }
    //public MenuVM Menu { get; set; }
}
