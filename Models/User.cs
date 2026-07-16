namespace Unstore.Models;

public class User : BaseModel
{
    public string Username { get; set; }
    public string Name { get; set; }
    public string PasswordHash { get; set; }
    public string Email { get; set; }
    public bool Active { get; set; }
    
    public CommercialUser? CommercialUser { get; set; } 
    public ICollection<Address> Addresses { get; set; } = new List<Address>();
    public ICollection<Role> Roles { get; set; } = new List<Role>();
    public ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
    public ICollection<ServiceRequest> ServicesRequests { get; set; } =  new List<ServiceRequest>();
    public ICollection<ServiceAvaliation> ServiceAvaliations { get; set; } = new List<ServiceAvaliation>();
    public ICollection<UserDocument> UserDocuments { get; set; } = new List<UserDocument>();
    public ICollection<ProductAvaliation> ProductAvaliations { get; set; } = new List<ProductAvaliation>();
}