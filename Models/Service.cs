namespace Unstore.Models;

public class Service : BaseModel
{
    public int ClientId { get; set; }
    public ICollection<User> Clients { get; set; }
    public int EmployeeId { get; set; }
    public ICollection<User> Employee { get; set; }
    public ICollection<Product> Products { get; set; } = new List<Product>();
    public ICollection<Tool> Tools { get; set; } = new List<Tool>();
    public string Details { get; set; }
    public string Address { get; set; }
    public decimal Cost { get; set; }

    public Service()
    {

    }
}