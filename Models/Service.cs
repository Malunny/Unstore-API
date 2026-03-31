namespace Unstorekle.Models;

public class Service : BaseModel
{
    public int ClientId { get; set; }
    public Client Client { get; set; }
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; }
    public ICollection<Product> Products { get; set; } = new List<Product>();
    public ICollection<Tool> Tools { get; set; } = new List<Tool>();
    public string Details { get; set; }
    public string Address { get; set; }
}