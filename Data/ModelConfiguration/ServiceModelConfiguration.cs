using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Unstore.Models;

namespace Unstore.Data.ModelConfiguration;

public class ServiceModelConfiguration : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Description).IsRequired();
        builder.Property(x => x.Description)
            .HasMaxLength(500);
        
        builder.Property(x => x.Cost).IsRequired();
        builder.Property(x => x.Cost)
            .HasPrecision(2);

        builder.HasOne(x => x.Address)
            .WithMany()
            .HasForeignKey(x => x.AddressId);

        builder.HasMany(x => x.Clients)
            .WithMany(x => x.RequestedServices)
            .UsingEntity("UserServices");

        builder.HasMany(x => x.ServiceProviders)
            .WithMany(x => x.OfferedServices)
            .UsingEntity("ProviderServices");
    }
}
