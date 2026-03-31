namespace Unstorekle.Models;

public class Position : BaseModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Wage { get; set; }

    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}