using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using Unstore.Models;

namespace Unstore.Data;

public class AppDbContext : DbContext
{
    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }
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

        modelBuilder.Entity<Role>()
            .ToTable("Roles");
        modelBuilder.Entity<Role>()
            .HasKey(x => x.Id);
        modelBuilder.Entity<Role>()
            .Property(x => x.Name)
            .IsRequired()
            .HasColumnType("nvarchar(100)");
        modelBuilder.Entity<Role>()
            .Property(x => x.Description)
            .HasColumnType("nvarchar(500)")
            .IsRequired();
        
        // User
        modelBuilder.Entity<User>()
            .ToTable("Users");
        modelBuilder.Entity<User>()
            .Property(x => x.Username)
            .IsRequired()
            .HasColumnType("nvarchar(20)");
            modelBuilder.Entity<User>()
            .HasIndex(x => x.Username)
            .IsUnique();
        modelBuilder.Entity<User>()
            .Property(x => x.Password)
            .HasColumnName("PasswordHash")
            .IsRequired()
            .HasColumnType("nvarchar(400)");
        modelBuilder.Entity<User>()
            .Property(x => x.Email)
            .IsRequired()
            .HasColumnType("nvarchar(150)");
        modelBuilder.Entity<User>()
            .HasIndex(x => x.Email)
            .IsUnique();
        modelBuilder.Entity<User>()
            .Property(x => x.Name)
            .IsRequired()
            .HasColumnType("nvarchar(120)");
        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId);

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

        // ---------------------- DATA SEEDING ----------------------

        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Name = "Normal", Description = "Usuário comum" },
            new Role { Id = 2, Name = "ADM", Description = "Administrador total" },
            new Role { Id = 3, Name = "Vendedor", Description = "Acesso a vendas e clientes" },
            new Role { Id = 4, Name = "Instalador", Description = "Acesso a ordens de serviço e ferramentas" }
        );

        modelBuilder.Entity<Position>().HasData(
            new Position { Id = 1, Name = "Instalador Sênior", Description = "Especialista em persianas motorizadas", Wage = 3500.00m },
            new Position { Id = 2, Name = "Ajudante de Instalação", Description = "Auxílio em furações e transporte", Wage = 1800.00m },
            new Position { Id = 3, Name = "Consultor de Vendas", Description = "Atendimento interno e orçamentos", Wage = 2200.00m }
        );

        modelBuilder.Entity<Employee>().HasData(
            new Employee { Id = 1, Name = "Roberto Alves", Email = "roberto@unstore.com", ContactNumber = "11911111111", PositionId = 1, Active = true },
            new Employee { Id = 2, Name = "Felipe Souza", Email = "felipe@unstore.com", ContactNumber = "11922222222", PositionId = 2, Active = true },
            new Employee { Id = 3, Name = "Mariana Costa", Email = "mariana@unstore.com", ContactNumber = "11933333333", PositionId = 3, Active = true },
            new Employee { Id = 4, Name = "Lucas Mendes", Email = "lucas@unstore.com", ContactNumber = "11944444444", PositionId = 1, Active = true },
            new Employee { Id = 5, Name = "Beatriz Rocha", Email = "beatriz@unstore.com", ContactNumber = "11955555555", PositionId = 2, Active = false }
        );

        modelBuilder.Entity<Client>().HasData(
            new Client { Id = 1, Name = "Condomínio Solar", Email = "contato@solar.com", Address = "Av. Paulista, 1000", ContactNumber = "1130001000" },
            new Client { Id = 2, Name = "Residencial Harmonia", Email = "adm@harmonia.com", Address = "Rua das Flores, 50", ContactNumber = "1130002000" },
            new Client { Id = 3, Name = "Clínica Médica Bem Estar", Email = "atendimento@bemestar.com", Address = "Rua Saúde, 200", ContactNumber = "1130003000" },
            new Client { Id = 4, Name = "Padaria Central", Email = "pao@central.com", Address = "Praça da Sé, 10", ContactNumber = "1130004000" },
            new Client { Id = 5, Name = "Escritório Advocacia X", Email = "legal@advx.com", Address = "Alameda Santos, 450", ContactNumber = "1130005000" },
            new Client { Id = 6, Name = "João da Silva", Email = "joao@gmail.com", Address = "Rua B, 123", ContactNumber = "11988887777" },
            new Client { Id = 7, Name = "Maria Oliveira", Email = "maria@yahoo.com", Address = "Av. Brasil, 99", ContactNumber = "11977776666" },
            new Client { Id = 8, Name = "Restaurante Sabor", Email = "gerencia@sabor.com", Address = "Rua Gastronomia, 15", ContactNumber = "1130008000" },
            new Client { Id = 9, Name = "Escola Infantil Prime", Email = "diretoria@prime.com", Address = "Rua Educação, 88", ContactNumber = "1130009000" },
            new Client { Id = 10, Name = "Academia Fit", Email = "treino@fit.com", Address = "Rua do Suor, 500", ContactNumber = "1130010000" }
        );

        modelBuilder.Entity<ToolTag>().HasData(
            new ToolTag { Id = 1, TagName = "Elétrica", Description = "Ferramentas que usam bateria ou cabo" },
            new ToolTag { Id = 2, TagName = "Medição", Description = "Precisão e medidas" },
            new ToolTag { Id = 3, TagName = "Manual", Description = "Ferramentas de mão" }
        );

        modelBuilder.Entity<Tool>().HasData(
            new Tool { Id = 1, Name = "Furadeira de Impacto", Description = "Furadeira Makita 18V" },
            new Tool { Id = 2, Name = "Trena Laser", Description = "Medidor Bosch 50m" },
            new Tool { Id = 3, Name = "Nível de Bolha", Description = "Nível de alumínio 60cm" },
            new Tool { Id = 4, Name = "Escada Extensível", Description = "Escada 7 degraus" },
            new Tool { Id = 5, Name = "Parafusadeira", Description = "DeWalt com controle de torque" },
            new Tool { Id = 6, Name = "Maleta de Chaves", Description = "Jogo de chaves Phillips e Fenda" },
            new Tool { Id = 7, Name = "Aspirador Portátil", Description = "Para limpeza pós-furação" },
            new Tool { Id = 8, Name = "Martelo de Borracha", Description = "Para ajuste de suportes" },
            new Tool { Id = 9, Name = "Estilete Profissional", Description = "Corte de sobras de tecido" },
            new Tool { Id = 10, Name = "Detector de Metais/Vigas", Description = "Para evitar furar canos" }
        );

        // Relacionamento Ferramenta x Tag
        modelBuilder.Entity("ToolToolTags").HasData(
            new { ToolId = 1, ToolTagId = 1 },
            new { ToolId = 5, ToolTagId = 1 },
            new { ToolId = 2, ToolTagId = 2 },
            new { ToolId = 3, ToolTagId = 2 },
            new { ToolId = 10, ToolTagId = 2 },
            new { ToolId = 6, ToolTagId = 3 },
            new { ToolId = 8, ToolTagId = 3 },
            new { ToolId = 9, ToolTagId = 3 }
        );

        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Persiana Rolo Blackout", Description = "Tecido 100% poliéster", Value = 150.00m, ImageUrl = "url.com" },
            new Product { Id = 2, Name = "Cortina de Linho", Description = "Tecido sob medida", Value = 280.00m, ImageUrl = "url.com" },
            new Product { Id = 3, Name = "Motor para Persiana", Description = "Somfy 220v", Value = 450.00m, ImageUrl = "url.com" }
        );

        modelBuilder.Entity<Service>().HasData(
            new Service 
            { 
                Id = 1, 
                ClientId = 1, 
                EmployeeId = 1, 
                Address = "Av. Paulista, 1000 - Sala 5", 
                Details = "Instalação de 4 persianas blackout motorizadas" 
            }
        );
    }
}