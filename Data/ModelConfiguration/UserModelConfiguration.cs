using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Unstore.Models;

namespace Unstore.Data.ModelConfiguration;

public class UserModelConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .IsRequired()
            .UseIdentityByDefaultColumn();
        
        builder.HasIndex(x => x.Username).IsUnique();
        builder.HasIndex(x => x.Email).IsUnique();
        
        builder.Property(x => x.Username)
            .IsRequired()
            .HasMaxLength(20);
        
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(80);
        
        builder.Property(x => x.PasswordHash)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasMany(x => x.Addresses)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId);

        builder.HasMany(x => x.Purchases)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.UserDocuments)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId);

        builder.HasMany(x => x.ServiceAvaliations)
            .WithOne(y => y.Client)
            .HasForeignKey(y => y.ClientId);
        
        builder.HasMany(x => x.ServicesRequests)
            .WithOne(y => y.Requester)
            .HasForeignKey(x => x.RequesterId);
    }
}