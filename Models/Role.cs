namespace Unstore.Models;

public class Role : BaseModel
{
    public string Name { get; set; } 
    public string Description { get; set; }
    public ICollection<User> Users { get; set; } = new List<User>();
}