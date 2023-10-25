using System.ComponentModel.DataAnnotations;

namespace XoomCore.Application.RequestModels.AccessControl;

public class SaveRoleRequest
{
    public long Id { get; set; }
    [Required]
    public string Name { get; set; }
    public string Description { get; set; }
    public int Status { get; set; }
}
