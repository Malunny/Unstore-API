namespace Unstorekle.Models;

public class Tool : BaseModel
{
    public string Name { get; set; }
    public string Description { get; set; }

    public ICollection<ToolTag> ToolTags { get; set; } = new List<ToolTag>();

    public Tool()
    {
        
    }

    public Tool(string name, string description, ICollection<ToolTag> toolTags)
    {
        Name = name;
        Description = description;
        
        ToolTags = toolTags;
    }
}