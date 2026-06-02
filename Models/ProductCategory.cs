namespace Unstore.Models;

public class ProductCategory : BaseModel
{
    public string Key { get; set; }
    public string Description { get; set; }
    public ICollection<Product> Products { get; set; } = new List<Product>();
}