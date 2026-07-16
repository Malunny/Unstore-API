namespace Unstore.Models;

public class DocumentType : BaseModel
{
    public string Key { get; set; }
    public string Description { get; set; }

    public ICollection<UserDocument> UserDocuments { get; set; }
}