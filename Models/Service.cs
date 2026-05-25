namespace Unstore.Models;

public class Service : BaseModel
{
    public string Description { get; set; }
    public string Address { get; set; }
    public decimal Cost { get; set; }

    public ICollection<User> Clients { get; set; }
    public ICollection<User> ServiceProviders { get; set; }
    public Service()
    {

    }
}