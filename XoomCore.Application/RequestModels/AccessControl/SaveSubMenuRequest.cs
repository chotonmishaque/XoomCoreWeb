using System.ComponentModel.DataAnnotations;

namespace XoomCore.Application.RequestModels.AccessControl;

public class SaveSubMenuRequest
{
    public long Id { get; set; }
    public long MenuId { get; set; }
    [Required]
    public string Key { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Url { get; set; }
    public int DisplaySequence { get; set; }
    public int Status { get; set; }
}
