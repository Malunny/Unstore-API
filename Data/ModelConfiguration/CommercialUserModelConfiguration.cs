using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Unstore.Models;

namespace Unstore.Data.ModelConfiguration;

public class CommercialUserModelConfiguration : IEntityTypeConfiguration<CommercialUser>
{
    public void Configure(EntityTypeBuilder<CommercialUser> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.ComercialName).IsRequired();
        builder.Property(x => x.ComercialName)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(100);

        builder.HasOne(x => x.OriginalUser)
            .WithOne(y => y.CommercialUser);
    }
}
