using XoomCore.Application.ViewModels.AccessControl;

namespace XoomCore.Application.ViewModels;

public class SubMenuAuthorizedVM
{
    public long Id { get; set; }
    public string Key { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
    public int DisplaySequence { get; set; }
    public bool ActiveMenuItem { get; set; } = false;
    public MenuVM Menu { get; set; }
}
