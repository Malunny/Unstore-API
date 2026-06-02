namespace Unstore.Models;

public class ComercialUser : BaseModel
{
    public string ComercialName { get; set; }
    
    public int OriginalUserId { get; set; }
    public User OriginalUser { get; set; }
    public ICollection<Product> SellingProducts { get; set; }
    public ICollection<Service> OfferedServices { get; set; }
}