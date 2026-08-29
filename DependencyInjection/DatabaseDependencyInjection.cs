using Microsoft.EntityFrameworkCore;
using Unstore.Data;

namespace Unstore.DependencyInjection;

public static class DatabaseDependencyInjection
{
    public static void AddDatabaseServices(this IServiceCollection services, WebApplicationBuilder builder, bool development = false)
    {
        string? connectionString = builder.Configuration.GetConnectionString("UnstoredbCloud");
        
        if (development)
            connectionString = builder.Configuration.GetConnectionString("UnstoredbLocal");
        
        services.AddDbContext<AppDbContext>((sp, options) =>
        {
            options.UseNpgsql(connectionString);
        });
    }
}