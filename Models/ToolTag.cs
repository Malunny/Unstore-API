namespace Unstorekle.Models;

public class ToolTag : BaseModel
{
    public string TagName { get; set; }
    public string Description { get; set; }

    public ICollection<Tool> Tools { get; set; } = new List<Tool>();
}