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
            .HasColumnType("INT")
            .UseAutoincrement();

        builder.Property(x => x.Street).IsRequired();
        builder.Property(x => x.Street)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(150);
        
        builder.Property(x => x.City).IsRequired();
        builder.Property(x => x.City)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(150);
        
        builder.Property(x => x.State).IsRequired();
        builder.Property(x => x.State)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(100);
        
        builder.Property(x => x.Number).IsRequired();
        builder.Property(x => x.Number)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(10);
        
        builder.Property(x => x.ZipCode).IsRequired();
        builder.Property(x => x.ZipCode)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(25);
        
        builder.Property(x => x.Complement).IsRequired(false);
        builder.Property(x => x.Complement)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(50);

        builder.HasOne(x => x.User)
            .WithMany(x => x.Addresses)
            .HasForeignKey(x => x.UserId);

        builder.HasOne(x => x.Type)
            .WithMany()
            .HasForeignKey(x => x.TypeId);
    }
}
