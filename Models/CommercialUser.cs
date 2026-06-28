namespace Unstore.Models;

public class CommercialUser : BaseModel
{
    public string CommercialName { get; set; }
    
    public int OriginalUserId { get; set; }
    public string About { get; set; }
    public bool Active { get; set; }
    public User OriginalUser { get; set; }
    public ICollection<Purchase> Sales { get; set; }
    public ICollection<Product> SellingProducts { get; set; }
    public ICollection<Service> OfferedServices { get; set; }
}