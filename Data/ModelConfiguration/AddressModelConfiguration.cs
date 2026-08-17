using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Unstore.Models;

namespace Unstore.Data.ModelConfiguration;

public class AddressModelConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .UseIdentityByDefaultColumn();

        builder.Property(x => x.Street)
            .IsRequired()
            .HasMaxLength(150);
        
        builder.Property(x => x.City)
            .IsRequired()
            .HasMaxLength(150);
        
        builder.Property(x => x.State)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(x => x.Number)
            .IsRequired()
            .HasMaxLength(10);
        
        builder.Property(x => x.ZipCode)
            .IsRequired()
            .HasMaxLength(25);
        
        builder.Property(x => x.Complement)
            .IsRequired(false)
            .HasMaxLength(50);

        builder.HasOne(x => x.Type)
            .WithMany()
            .HasForeignKey(x => x.TypeId);
    }
}
