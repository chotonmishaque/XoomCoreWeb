using XoomCore.Core.Enum;

namespace XoomCore.Application.ViewModels.AccessControl;

public class UserVM
{
    public long Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string PhoneNumber { get; set; }
    public UserStatus Status { get; set; }
}
