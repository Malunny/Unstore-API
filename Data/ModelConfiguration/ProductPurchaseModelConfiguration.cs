using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Unstore.Models;

namespace Unstore.Data.ModelConfiguration;

public class ProductPurchaseModelConfiguration : IEntityTypeConfiguration<ProductPurchase>
{
    public void Configure(EntityTypeBuilder<ProductPurchase> builder)
    {
        builder.ToTable("ProductPurchases");
        builder.HasKey(x => new { x.ProductId, x.PurchaseId });
    }
}