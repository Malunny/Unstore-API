namespace Unstore.DependencyInjection;

public static class DevelopmentDependencyInjection
{
    public static void AddDevelopmentServices(this IServiceCollection services)
    {
        services
            .AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.WriteIndented = true;
                options.JsonSerializerOptions.ReferenceHandler =
                    System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
            })
            .ConfigureApiBehaviorOptions(options => { options.SuppressModelStateInvalidFilter = true; });
    }
}