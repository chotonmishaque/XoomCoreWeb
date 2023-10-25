using System.ComponentModel.DataAnnotations;

namespace XoomCore.Application.RequestModels.AccessControl;

public class ChangeUserPasswordRequest
{
    public long Id { get; set; }
    [Required]
    public string NewPassword { get; set; }

}
