namespace Unstore.Models;

public class Address : BaseModel
{
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Number { get; set; }
    public string ZipCode { get; set; }
    public string? Complement { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }
    public int TypeId { get; set; }
    public AddressType Type { get; set; }
}