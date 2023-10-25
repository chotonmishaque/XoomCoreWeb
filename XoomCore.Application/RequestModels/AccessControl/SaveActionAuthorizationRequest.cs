using System.ComponentModel.DataAnnotations;

namespace XoomCore.Application.RequestModels.AccessControl;

public class SaveActionAuthorizationRequest
{
    public long Id { get; set; }
    public long SubMenuId { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Controller { get; set; }
    [Required]
    public string ActionMethod { get; set; }
    public string? Description { get; set; }
    public int IsPageLinked { get; set; }
    public int Status { get; set; }
}
