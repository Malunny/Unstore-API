namespace Unstore.Models;

public class Purchase : BaseModel
{
    public DateTime BoughtDate { get; set; }
    public decimal TotalValue { get; set; }

    public int AddressId { get; set; }
    public Address Address { get; set; }
    public ICollection<Product> Products { get; set; }
    public int ClientId { get; set; }
    public User Client { get; set; }
    public int SellerId { get; set; }
    public User Seller { get; set; }
}