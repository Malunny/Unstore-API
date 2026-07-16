namespace Unstore.Models;

public class ServiceOption : BaseModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int ServiceId { get; set; }
    public Service Service { get; set; }
}