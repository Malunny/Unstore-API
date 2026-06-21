using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Unstore.Models;

namespace Unstore.Data.ModelConfiguration;

public class PurchaseModelConfiguration : IEntityTypeConfiguration<Purchase>
{
    public void Configure(EntityTypeBuilder<Purchase> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .IsRequired()
            .UseAutoincrement();

        builder.Property(x => x.BoughtDate).IsRequired();
        
        builder.Property(x => x.TotalValue)
            .IsRequired()
            .HasColumnType("DECIMAL")
            .HasPrecision(18, 2);
        
        builder.HasOne(x => x.Address)
            .WithMany(y => y.SentPurchases)
            .HasForeignKey(x => x.AddressId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.User)
            .WithMany(x => x.Purchases)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(x => x.ProductPurchases)
            .WithOne(x => x.Purchase)
            .HasForeignKey(x => x.PurchaseId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
