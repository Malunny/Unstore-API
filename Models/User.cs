namespace Unstore.Models;

public class User : BaseModel
{
    public string Username { get; set; }
    public string Name { get; set; }
    public string PasswordHash { get; set; }
    public string Email { get; set; }
    public bool Active { get; set; }
    
    public CommercialUser? CommercialUser { get; set; } 
    public ICollection<Address> Addresses { get; set; }
    public ICollection<Role> Roles { get; set; } = new List<Role>();
    public ICollection<Purchase> Purchases { get; set; }
    public ICollection<Service> RequestedServices { get; set; }
    public ICollection<UserDocument> UserDocuments { get; set; }
}