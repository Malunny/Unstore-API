namespace Unstorekle.Models;

public class Client : BaseModel
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public string ContactNumber { get; set; }
    public ICollection<Service> Services { get; set; } = new List<Service>();
}