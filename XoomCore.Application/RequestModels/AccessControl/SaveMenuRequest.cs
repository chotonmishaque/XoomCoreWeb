using System.ComponentModel.DataAnnotations;

namespace XoomCore.Application.RequestModels.AccessControl;

public class SaveMenuRequest
{
    public long Id { get; set; }
    [Required]
    public string Name { get; set; }
    public string Description { get; set; }
    public int DisplaySequence { get; set; }
    [Required]
    public string Icon { get; set; }
    public int Status { get; set; }
}
