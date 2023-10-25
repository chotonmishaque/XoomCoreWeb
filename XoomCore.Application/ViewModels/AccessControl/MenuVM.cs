using XoomCore.Core.Enum;

namespace XoomCore.Application.ViewModels.AccessControl;

public class MenuVM
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int DisplaySequence { get; set; }
    public string Icon { get; set; }
    public EntityStatus Status { get; set; }
}
