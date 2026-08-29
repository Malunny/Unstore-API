using Unstore.Services;
using Unstore.Services.Account;
using Unstore.Services.CommercialUser;
using Unstore.Services.Product;
using Unstore.Services.User;

namespace Unstore.DependencyInjection;

public static class MainDependencyInjection
{
    public static void AddMainServices(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
    
        services.AddScoped<UserService>();
        services.AddScoped<ProductService>();
        services.AddScoped<AccountService>();
        services.AddScoped<CommercialAccountService>();
        services.AddScoped<CommercialUserActionService>();
        services.AddScoped<UserVerificationService>();
        services.AddScoped<UserPurchaseService>();
        services.AddSingleton<IServiceResultFactory, DataServiceResultFactory>();
        services.AddTransient<ITokenService, JwtTokenService>();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddMemoryCache();
        
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