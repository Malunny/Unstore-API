namespace Unstore.Models;

public class Purchase : BaseModel
{
    public DateTime BoughtDate { get; set; }
    public decimal TotalValue { get; set; }

    public int AddressId { get; set; }
    public Address Address { get; set; }
    public ICollection<ProductPurchase> ProductPurchases { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
}