namespace Unstore.Models;

public class Service : BaseModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal LowestPrice { get; set; }
    public DateOnly AvailableAt { get; set; }
    public bool Active { get; set; }
    
    public int ProviderId { get; set; }
    public CommercialUser Provider { get; set; }
    public ICollection<ServiceAvaliation> Avaliations { get; set; } = new List<ServiceAvaliation>();
    public ICollection<ServiceRequest> ServiceRequests { get; set; } = new List<ServiceRequest>();
    public ICollection<ServiceOption> ServiceOptions { get; set; } = new List<ServiceOption>();
    public Service()
    {

    }
}