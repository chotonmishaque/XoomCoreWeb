using System.ComponentModel.DataAnnotations;

namespace XoomCore.Application.RequestModels;

public class SignInRequest
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}
