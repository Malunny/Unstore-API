namespace Unstore.Models;

public class Client : BaseModel
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public string ContactNumber { get; set; }
    public ICollection<Service> Services { get; set; } = new List<Service>();

    public Client()
    {
        
    }
    
    public Client(string name, string address, string email,
        string contactNumber, ICollection<Service> services)
    {
        Name = name;
        Address = address;
        Email = email;
        ContactNumber = contactNumber;
        Services = services;
    }
}