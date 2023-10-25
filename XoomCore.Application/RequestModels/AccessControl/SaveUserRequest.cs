using System.ComponentModel.DataAnnotations;

namespace XoomCore.Application.RequestModels.AccessControl;

public class SaveUserRequest
{
    public long Id { get; set; }
    [Required]
    public string Username { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string FullName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    [Required]
    public string PhoneNumber { get; set; }
    public int Status { get; set; }
}
