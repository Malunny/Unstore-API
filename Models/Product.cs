namespace Unstore.Models;

public class Product : BaseModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Value { get; set; }
    public DateTime PublishedDate { get; set; } = DateTime.Now;
    public bool Active { get; set; }
    
    public ICollection<ProductAvaliation>? Avaliations { get; set; }
    public ICollection<ProductCategory> Categories { get; set; } = new List<ProductCategory>();
    public ICollection<Purchase> Purchases { get; set; }
    public ICollection<ProductPurchase> ProductPurchases { get; set; }
    
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