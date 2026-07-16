using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Unstore.Models;

namespace Unstore.Data.ModelConfiguration;

public class ServiceModelConfiguration : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .IsRequired()
            .UseAutoincrement();

        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(500);
        
        builder.Property(x => x.LowestPrice)
            .IsRequired()
            .HasColumnType("DECIMAL")
            .HasPrecision(18, 2);

        builder.HasOne(x => x.Provider)
            .WithMany(y => y.OfferedServices)
            .HasForeignKey(x => x.ProviderId);
        
        builder.HasMany(x => x.Avaliations)
            .WithOne(y => y.Service)
            .HasForeignKey(y => y.ServiceId);
        
        builder.HasMany(x => x.ServiceOptions)
            .WithOne(y => y.Service)
            .HasForeignKey(y => y.ServiceId);
        
        builder.HasMany(x => x.ServiceRequests)
            .WithOne(y => y.Service)
            .HasForeignKey(x => x.ServiceId);
    }
}
