using System.Net;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
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
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlite("Data Source=app.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}