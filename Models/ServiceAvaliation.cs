namespace Unstore.Models;

public class ServiceAvaliation
{
    public int ClientId { get; set; }
    public User Client { get; set; }
    
    public int ServiceId { get; set; }
    public Service Service { get; set; }
    
    public short Stars { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}