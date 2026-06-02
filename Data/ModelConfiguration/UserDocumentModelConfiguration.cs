using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Unstore.Models;

namespace Unstore.Data.ModelConfiguration;

public class UserDocumentModelConfiguration : IEntityTypeConfiguration<UserDocument>
{
    public void Configure(EntityTypeBuilder<UserDocument> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Information).IsRequired();

        builder.HasOne(x => x.User)
            .WithMany(x => x.UserDocuments)
            .HasForeignKey(x => x.UserId);

        builder.HasOne(x => x.DocumentType)
            .WithMany()
            .HasForeignKey(x => x.DocumentTypeId);
    }
}
