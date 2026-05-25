namespace Unstore.Models;

public class UserDocument : BaseModel
{
    public int UserId { get; set; }
    public User User { get; set; }
    public string Information { get; set; }
    public int DocumentTypeId { get; set; }
    public DocumentType DocumentType { get; set; }
}