using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Unstore.Models;

namespace Unstore.Data.ModelConfiguration;

public class ProductAvaliationModelConfiguration : IEntityTypeConfiguration<ProductAvaliation>
{
    public void Configure(EntityTypeBuilder<ProductAvaliation> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Description).IsRequired();
        builder.Property(x => x.Description)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(500);
        
        builder.Property(x => x.Stars).IsRequired();
        builder.Property(x => x.Stars)
            .HasColumnType("INT");

        builder.HasOne(x => x.Client)
            .WithMany()
            .HasForeignKey(x => x.UserId);

        builder.HasOne(x => x.Product)
            .WithMany(x => x.Avaliations)
            .HasForeignKey(x => x.ProductId);
    }
}
