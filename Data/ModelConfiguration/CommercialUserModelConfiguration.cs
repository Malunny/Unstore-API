using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Unstore.Models;

namespace Unstore.Data.ModelConfiguration;

public class CommercialUserModelConfiguration : IEntityTypeConfiguration<CommercialUser>
{
    public void Configure(EntityTypeBuilder<CommercialUser> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .IsRequired()
            .UseIdentityByDefaultColumn();
        
        builder.Property(x => x.CommercialName)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(x => x.About)
            .IsRequired()
            .HasMaxLength(500);
        
        builder.HasOne(x => x.OriginalUser)
            .WithOne(y => y.CommercialUser)
            .HasForeignKey<CommercialUser>(x => x.OriginalUserId);
    }
}
