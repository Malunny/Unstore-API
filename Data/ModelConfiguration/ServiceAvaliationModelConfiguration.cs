using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Unstore.Models;

namespace Unstore.Data.ModelConfiguration;

public class ServiceAvaliationModelConfiguration : IEntityTypeConfiguration<ServiceAvaliation>
{
    public void Configure(EntityTypeBuilder<ServiceAvaliation> builder)
    {
        builder.HasKey(x => new {x.ClientId, x.ServiceId});

        builder.Property(x => x.Stars)
            .IsRequired();
        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(500);
    }
}
