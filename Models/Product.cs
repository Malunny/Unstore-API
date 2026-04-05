namespace Unstore.Models;

public class Product : BaseModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Value { get; set; }
    public string ImageUrl { get; set; }

    public Product()
    {
        
    }

    public Product(string name, string description, decimal value, string imageUrl)
    {
        Name = name;
        Description = description;
        Value = value;
        ImageUrl = imageUrl;
    }
}