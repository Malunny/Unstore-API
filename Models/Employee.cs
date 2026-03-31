namespace Unstorekle.Models;

public class Employee : BaseModel
{
    public string Name { get; set; }
    public string ContactNumber { get; set; }
    public string Email { get; set; }
    public int PositionId { get; set; }
    public Position Position { get; set; }
    public DateTime StartedAt { get; set; }
    public bool Active { get; set; }

    public ICollection<Service> Services { get; set; } = new List<Service>();

    public Employee()
    {
        
    }
    public Employee(string name, string contactNumber, string email,
        Position position, DateTime startedAt, ICollection<Service> services, bool active = true)
    {
        Name = name;
        ContactNumber = contactNumber;
        Email = email;
        
        Position = position;
        PositionId = position.Id;
        
        StartedAt = startedAt;
        Active = active;
        
        Services = services;
    }
}