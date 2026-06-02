using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Unstore.Models;

namespace Unstore.Data.ModelConfiguration;

public class ServiceAvaliationModelConfiguration : IEntityTypeConfiguration<ServiceAvaliation>
{
    public void Configure(EntityTypeBuilder<ServiceAvaliation> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Stars).IsRequired();
        builder.Property(x => x.Description).IsRequired();
        builder.Property(x => x.Description)
            .HasMaxLength(500);

        builder.HasOne(x => x.Client)
            .WithMany()
            .HasForeignKey(x => x.UserId);

        builder.HasOne(x => x.Service)
            .WithMany()
            .HasForeignKey(x => x.ServiceId);
    }
}
