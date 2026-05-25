namespace Unstore.Models;

public class ProductAvaliation : BaseModel
{
    public string Description { get; set; }
    public short Stars { get; set; }
    
    public int UserId { get; set; }
    public User Client { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
}