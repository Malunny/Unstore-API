using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Unstore.Models;

namespace Unstore.Data.ModelConfiguration;

public class ProductAvaliationModelConfiguration : IEntityTypeConfiguration<ProductAvaliation>
{
    public void Configure(EntityTypeBuilder<ProductAvaliation> builder)
    {
        builder.HasKey(x => new { x.UserId, x.ProductId });

        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(500);
        
        builder.Property(x => x.Stars)
            .IsRequired()
            .HasColumnType("TINYINT");

        builder.HasOne(x => x.Client)
            .WithMany(y => y.ProductAvaliations)
            .HasForeignKey(x => x.UserId);

        builder.HasOne(x => x.Product)
            .WithMany(x => x.Avaliations)
            .HasForeignKey(x => x.ProductId);
    }
}
