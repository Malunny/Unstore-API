using System.Net;
using Microsoft.EntityFrameworkCore;
using Unstore.Models;

namespace Unstore.Data;

public class AppDbContext : DbContext
{
    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<CommercialUser> CommercialUsers { get; set; }
    
    public DbSet<UserDocument> UserDocuments { get; set; }
    public DbSet<DocumentType> DocumentTypes { get; set; }
    
    public DbSet<Address> Addresses { get; set; }
    public DbSet<AddressType> AddressTypes { get; set; }
    
    public DbSet<Service> Services { get; set; }
    public DbSet<ServiceAvaliation> ServiceAvaliations { get; set; }
    public DbSet<ServiceRequest> ServiceRequests { get; set; }
    public DbSet<ServiceOption> ServiceOptions { get; set; }

    public DbSet<Purchase> Purchases { get; set; }
    
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<ProductAvaliation> ProductAvaliations { get; set; }
    public DbSet<ProductPurchase> ProductPurchases { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> optionsBuilder) : base(optionsBuilder)
    {
        
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.UseSeeding((context, _) =>
        {
            var roleHasData = context.Set<Role>().Any();
            
            if (!roleHasData)
            {
                context.Set<Role>().AddRange([
                    new Role { Name = "Normal", Description = "Buying, Search and other features. A Regular normal user."},
                    new Role { Name = "Seller", Description = "Posting, Selling and managing Products." },
                    new Role { Name = "ServiceProvider", Description = "Posting, Selling and managing Services." },
                    new Role { Name = "Manager", Description = "Managing Products, Users, Interactions." },
                    new Role { Name = "Administrator", Description = "Administrating Eveything." },
                ]);
                
                context.SaveChanges();
                var adminRoleTracked = context.Set<Role>().First(role => role.Name == "Administrator");
        
                context.Set<User>().Add(new User {
                    Username = "Administration",
                    Name = "Administration",
                    Email = "admin.unstore@unstore.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin"),
                    Roles = [ adminRoleTracked ]
                });
            }
            
            var addressTypeHasData = context.Set<AddressType>().Any();
            
            if (!addressTypeHasData)
            {
                context.Set<AddressType>().AddRange([
                    new AddressType { Key = "Home", Description = "Your home, apartment or property."},
                    new AddressType { Key = "Work", Description = "For your workplace."},
                ]);
            }
            
            context.SaveChanges();
        });
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}