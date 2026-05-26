namespace Unstore.Models;

public class Product : BaseModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Value { get; set; }
    public DateTime PublishedDate { get; set; } = DateTime.Now;
    
    public ProductAvaliation? Avaliation { get; set; }
    public ICollection<ProductCategory> Categories { get; set; } = new List<ProductCategory>();
    
    public Product()
    {
        
    }

    public Product(string name, string description, decimal value)
    {
        Name = name;
        Description = description;
        Value = value;
    }
}