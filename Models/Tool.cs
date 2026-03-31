namespace Unstorekle.Models;

public class Tool : BaseModel
{
    public string Name { get; set; }
    public string Description { get; set; }

    public ICollection<ToolTag> ToolTags { get; set; } = new List<ToolTag>();
}