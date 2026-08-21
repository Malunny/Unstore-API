using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Unstore.Models;

namespace Unstore.Data.ModelConfiguration;

public class ServiceOptionModelConfiguration : IEntityTypeConfiguration<ServiceOption>
{
    public void Configure(EntityTypeBuilder<ServiceOption> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .IsRequired()
            .UseIdentityByDefaultColumn();

        builder.Property(x => x.Title)
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property(x => x.Description)
            .HasMaxLength(500)
            .IsRequired();
    }
}