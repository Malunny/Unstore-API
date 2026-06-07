using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Unstore.Models;

namespace Unstore.Data.ModelConfiguration;

public class ServiceRequestModelConfiguration : IEntityTypeConfiguration<ServiceRequest>
{
    public void Configure(EntityTypeBuilder<ServiceRequest> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.RequestedAt)
            .IsRequired();
        
        builder.Property(x => x.RequestedToDay)
            .IsRequired();
    }
}