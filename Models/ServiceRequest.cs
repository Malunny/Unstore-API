namespace Unstore.Models;

public class ServiceRequest : BaseModel
{
    public Service Service { get; set; }
    public int ServiceId { get; set; }
    public User Requester { get; set; }
    public int RequesterId { get; set; }
    public DateTime RequestedAt { get; set; }
    public DateTime RequestedToDay { get; set; }
}