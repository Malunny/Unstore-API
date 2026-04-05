using Unstore.Models;

namespace Unstore.Data;

public static class DbDataSeeder
{
    public static void Seed(AppDbContext context)
    {
        context.Database.EnsureCreated();

        if (context.Positions.Any()) return;
        
        // --- x Roles ---

        var roles = new List<Role>()
        {
            new Role() { Name = "Admin", Description = "Full access to all system features and settings." },
            new Role() { Name = "Manager", Description = "Can manage employees, view reports." },
            new Role() { Name = "User", Description = "For Costumers and normal Access." }
        };

        context.Roles.AddRange(roles);
        context.SaveChanges();
        
        // --- 1. POSITIONS ---
        var positions = new List<Position>
        {
            new Position { Name = "Senior Creative Director", Description = "Liderança de projetos visuais", Wage = 8500.00m },
            new Position { Name = "Mid-Level Developer", Description = "Desenvolvimento de sistemas core", Wage = 6200.00m },
            new Position { Name = "Lead Tattoo Artist", Description = "Especialista em fine line e blackwork", Wage = 5500.00m },
            new Position { Name = "Junior Designer", Description = "Suporte em peças gráficas", Wage = 3200.00m },
            new Position { Name = "Operations Manager", Description = "Gestão de fluxo e estoque", Wage = 4800.00m }
        };
        context.Positions.AddRange(positions);
        context.SaveChanges();

        // --- 2. EMPLOYEES ---
        var employees = new List<Employee>
        {
            new Employee { Name = "Alex Rivera", Email = "alex.rivera@unstorekle.com", ContactNumber = "11912345678", PositionId = positions[0].Id, StartedAt = DateTime.Now.AddYears(-2) },
            new Employee { Name = "Jordan Smith", Email = "jordan.s@unstorekle.com", ContactNumber = "11923456789", PositionId = positions[1].Id, StartedAt = DateTime.Now.AddMonths(-8) },
            new Employee { Name = "Casey Wong", Email = "casey.w@unstorekle.com", ContactNumber = "11934567890", PositionId = positions[2].Id, StartedAt = DateTime.Now.AddYears(-1) },
            new Employee { Name = "Taylor Reed", Email = "taylor.r@unstorekle.com", ContactNumber = "11945678901", PositionId = positions[3].Id, StartedAt = DateTime.Now.AddMonths(-3) },
            new Employee { Name = "Morgan Lane", Email = "morgan.l@unstorekle.com", ContactNumber = "11956789012", PositionId = positions[4].Id, StartedAt = DateTime.Now.AddMonths(-15) }
        };
        context.Employees.AddRange(employees);
        context.SaveChanges();

        // --- 3. CLIENTS ---
        var clients = new List<Client>
        {
            new Client { Name = "Quantum Tech Solutions", Address = "Tech Avenue, 404", Email = "contact@quantum.com", ContactNumber = "1130001000" },
            new Client { Name = "Aura Studio", Address = "Vibe Street, 77", Email = "hello@aura.design", ContactNumber = "1130002000" },
            new Client { Name = "Nexus Logistics", Address = "Industrial Port, 12", Email = "ops@nexus.log", ContactNumber = "1130003000" },
            new Client { Name = "Nova Cosmetics", Address = "Beauty Boulevard, 99", Email = "marketing@nova.com", ContactNumber = "1130004000" },
            new Client { Name = "Private Collector X", Address = "Secret Location", Email = "pcx@secure.com", ContactNumber = "1130005000" }
        };
        context.Clients.AddRange(clients);
        context.SaveChanges();

        // --- 4. TOOL TAGS ---
        var tags = new List<ToolTag>
        {
            new ToolTag { TagName = "Precision", Description = "Ferramentas de alta precisão" },
            new ToolTag { TagName = "Heavy Duty", Description = "Equipamentos de uso intenso" },
            new ToolTag { TagName = "Digital", Description = "Ferramentas de hardware computacional" },
            new ToolTag { TagName = "Sanitized", Description = "Materiais com protocolo de esterilização" }
        };
        context.ToolTags.AddRange(tags);
        context.SaveChanges();

        // --- 5. TOOLS ---
        var tools = new List<Tool>
        {
            new Tool { Name = "Wacom Cintiq Pro", Description = "Display interativo para design", ToolTags = new List<ToolTag> { tags[0], tags[2] } },
            new Tool { Name = "Wireless Rotary Pen", Description = "Máquina de tatuagem topo de linha", ToolTags = new List<ToolTag> { tags[0], tags[3] } },
            new Tool { Name = "MacStudio M2 Ultra", Description = "Workstation para processamento pesado", ToolTags = new List<ToolTag> { tags[1], tags[2] } },
            new Tool { Name = "Thermal Stencil Printer", Description = "Impressora térmica de decalques", ToolTags = new List<ToolTag> { tags[2], tags[3] } }
        };
        context.Tools.AddRange(tools);
        context.SaveChanges();

        // --- 6. PRODUCTS ---
        var products = new List<Product>
        {
            new Product { Name = "Premium Black Ink 500ml", Description = "Pigmento hipoalergênico", Value = 250.00m, ImageUrl = "ink_01.jpg" },
            new Product { Name = "Needle Cartridge 05RL", Description = "Pack com 20 unidades", Value = 120.00m, ImageUrl = "cart_05rl.jpg" },
            new Product { Name = "Design Asset Pack v1", Description = "Licença de texturas exclusivas", Value = 45.00m, ImageUrl = "assets.jpg" },
            new Product { Name = "Aftercare Balm", Description = "Pomada regeneradora 30g", Value = 35.50m, ImageUrl = "balm.jpg" }
        };
        context.Products.AddRange(products);
        context.SaveChanges();

        // --- 7. SERVICES ---
        var services = new List<Service>
        {
            new Service 
            { 
                ClientId = clients[0].Id, 
                EmployeeId = employees[1].Id, 
                Details = "Full System Architecture Review", 
                Address = clients[0].Address,
                Products = new List<Product> { products[2] },
                Tools = new List<Tool> { tools[2] }
            },
            new Service 
            { 
                ClientId = clients[1].Id, 
                EmployeeId = employees[0].Id, 
                Details = "Branding Refresh and Visual Guidelines", 
                Address = "Remote / Online",
                Products = new List<Product> { products[2] },
                Tools = new List<Tool> { tools[0], tools[2] }
            },
            new Service 
            { 
                ClientId = clients[4].Id, 
                EmployeeId = employees[2].Id, 
                Details = "Custom Sleeve Tattoo - Session 01", 
                Address = "Studio Central",
                Products = new List<Product> { products[0], products[1] },
                Tools = new List<Tool> { tools[1], tools[3] }
            }
        };
        context.Services.AddRange(services);
        context.SaveChanges();
    }
}