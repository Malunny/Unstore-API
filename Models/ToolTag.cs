namespace Unstore.Models;

public class ToolTag : BaseModel
{
    public string TagName { get; set; }
    public string Description { get; set; }

    public ICollection<Tool> Tools { get; set; } = new List<Tool>();

    public ToolTag()
    {
        
    }

    public ToolTag(string tagName, string description, ICollection<Tool> tools)
    {
        TagName = tagName;
        Description = description;
        
        Tools = tools;
    }
}