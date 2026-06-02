using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Unstore.Models;

namespace Unstore.Data.ModelConfiguration;

public class AddressTypeModelConfiguration : IEntityTypeConfiguration<AddressType>
{
    public void Configure(EntityTypeBuilder<AddressType> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Key).IsRequired();
        builder.Property(x => x.Key)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(100);
        
        builder.Property(x => x.Description).IsRequired();
        builder.Property(x => x.Description)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(200);
    }
}
