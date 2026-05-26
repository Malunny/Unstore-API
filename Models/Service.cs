namespace Unstore.Models;

public class Service : BaseModel
{
    public string Description { get; set; }
    public decimal Cost { get; set; }

    public int AddressId { get; set; }
    public Address Address { get; set; }
    
    public ICollection<User> Clients { get; set; }
    public ICollection<User> ServiceProviders { get; set; }
    public Service()
    {

    }
}