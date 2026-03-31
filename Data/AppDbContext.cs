using Microsoft.EntityFrameworkCore;
using Unstorekle.Models;

namespace Unstorekle.Data;

public class AppDbContext : DbContext
{
    public DbSet<Client> Clients { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Tool> Tools { get; set; }
    public DbSet<ToolTag> ToolTags { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<Position> Positions { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlite("Data Source=app.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Client
        
        modelBuilder.Entity<Client>()
            .ToTable("Clients");

        modelBuilder.Entity<Client>().HasKey(x => x.Id);
        
        modelBuilder.Entity<Client>()
            .Property(x => x.Name)
            .IsRequired()
            .HasColumnType("nvarchar(150)");
        
        modelBuilder.Entity<Client>()
            .Property(x => x.Email)
            .IsRequired()
            .HasColumnType("nvarchar(150)");
        
        modelBuilder.Entity<Client>()
            .Property(x => x.Address)
            .IsRequired()
            .HasColumnType("nvarchar(150)");

        modelBuilder.Entity<Client>()
            .Property(x => x.ContactNumber)
            .IsRequired()
            .HasColumnType("varchar(15)");
        
        // Position
        modelBuilder.Entity<Position>().HasKey(x => x.Id);
        
        modelBuilder.Entity<Position>()
            .ToTable("Positions");
        
        modelBuilder.Entity<Position>()
            .Property(x => x.Name)
            .IsRequired()
            .HasColumnType("nvarchar(100)");
        
        modelBuilder.Entity<Position>()
            .Property(x => x.Description)
            .HasColumnType("nvarchar(500)");

        modelBuilder.Entity<Position>()
            .Property(x => x.Wage)
            .HasColumnType("decimal(10,2)");
        
        // Employee
        
        modelBuilder.Entity<Employee>().HasKey(x => x.Id);
        
        modelBuilder.Entity<Employee>()
            .ToTable("Employees");
        
        modelBuilder.Entity<Employee>()
            .Property(p => p.Name)
            .IsRequired()
            .HasColumnType("nvarchar(150)");
        
        modelBuilder.Entity<Employee>()
            .Property(p => p.Active)
            .HasColumnType("bit")
            .HasDefaultValue(true);

        modelBuilder.Entity<Employee>()
            .Property(p => p.StartedAt)
            .HasColumnType("datetime")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        
        modelBuilder.Entity<Employee>()
            .Property(x => x.ContactNumber)
            .IsRequired()
            .HasColumnType("varchar(15)");
        
        modelBuilder.Entity<Employee>()
            .Property(x => x.Email)
            .IsRequired()
            .HasColumnType("nvarchar(150)");
        
        // Product
        
        modelBuilder.Entity<Product>().HasKey(x => x.Id);
        
        modelBuilder.Entity<Product>()
            .ToTable("Products");

        modelBuilder.Entity<Product>()
            .Property(p => p.Name)
            .IsRequired()
            .HasColumnType("nvarchar(100)");
        
        modelBuilder.Entity<Product>()
            .Property(p => p.Description)
            .HasColumnType("nvarchar(500)");
        
        modelBuilder.Entity<Product>()
            .Property(p => p.ImageUrl)
            .HasColumnType("nvarchar(200)");
        
        modelBuilder.Entity<Product>()
            .Property(p => p.Value)
            .IsRequired()
            .HasColumnType("decimal(6,2)");
        
        // Service

        modelBuilder.Entity<Service>().HasKey(x => x.Id);
        
        modelBuilder.Entity<Service>()
            .ToTable("Services");
        
        modelBuilder.Entity<Service>()
            .Property(s => s.Address)
            .IsRequired()
            .HasColumnType("nvarchar(150)");

        modelBuilder.Entity<Service>()
            .Property(s => s.Details)
            .HasColumnType("nvarchar(500)");
        
        // Tool
        
        modelBuilder.Entity<Tool>().HasKey(x => x.Id);

        modelBuilder.Entity<Tool>()
            .ToTable("Tools");
        
        modelBuilder.Entity<Tool>()
            .Property(t => t.Name)
            .IsRequired()
            .HasColumnType("nvarchar(150)");

        modelBuilder.Entity<Tool>()
            .Property(t => t.Description)
            .HasColumnType("nvarchar(500)");
        
        // ToolTag
        
        modelBuilder.Entity<ToolTag>().HasKey(x => x.Id);
        
        modelBuilder.Entity<ToolTag>()
            .ToTable("ToolTags");
        
        modelBuilder.Entity<ToolTag>()
            .Property(tt => tt.TagName)
            .IsRequired()
            .HasColumnType("nvarchar(150)");

        modelBuilder.Entity<ToolTag>()
            .Property(tt => tt.Description)
            .HasColumnType("nvarchar(500)");
        
        // Position-Employee relationship
        modelBuilder.Entity<Position>()
            .HasMany(p => p.Employees)
            .WithOne(e => e.Position);
        
        // Tools Relationship
        modelBuilder.Entity<Tool>()
            .HasMany(t => t.ToolTags)
            .WithMany(tt => tt.Tools)
            .UsingEntity<Dictionary<string, object>>(
                "ToolToolTags",
                j => j.HasOne<ToolTag>().WithMany().HasForeignKey("ToolTagId"),
                j => j.HasOne<Tool>().WithMany().HasForeignKey("ToolId")
            );
        
        // Services relationships
        modelBuilder.Entity<Service>()
            .HasOne(s => s.Client)
            .WithMany(c => c.Services)
            .HasForeignKey(s => s.ClientId);

        modelBuilder.Entity<Service>()
            .HasOne(s => s.Employee)
            .WithMany(e => e.Services)
            .HasForeignKey(s => s.EmployeeId);

        modelBuilder.Entity<Service>()
            .HasMany(s => s.Products)
            .WithOne();

        modelBuilder.Entity<Service>()
            .HasMany(s => s.Tools)
            .WithMany();
    }
}