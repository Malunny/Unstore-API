using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Unstore.Models;

namespace Unstore.Data.ModelConfiguration;

public class ProductModelConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.Name)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(100);
        
        builder.Property(x => x.Description).IsRequired();
        builder.Property(x => x.Description)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(500);
        
        builder.Property(x => x.Value).IsRequired();
        builder.Property(x => x.Value)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(100);
        
        builder.Property(x => x.PublishedDate).IsRequired();

        builder.HasMany(x => x.Avaliations)
            .WithOne()
            .HasForeignKey(x => x.ProductId);

        builder.HasMany(x => x.Categories)
            .WithMany(x => x.Products)
            .UsingEntity(x => x.ToTable("ProductProductCategories"));
    }
}
