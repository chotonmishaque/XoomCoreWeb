namespace XoomCore.Application.ViewModels;

public class SessionCreationResponseVM
{
    public long UserId { get; set; }
    public string Username { get; set; }
    public List<ActionAuthorizedVM> ActionAuthorizedList { get; set; }
    public List<SubMenuAuthorizedVM> SubMenuAuthorizedList { get; set; }
}