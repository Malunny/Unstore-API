namespace Unstore;

public static class Configuration
{
    // TOKEN - Json Web Token
    public static string JwtKey { get; set; }
    public static string ApiKey { get; set; }
    public static int TokenExpirationTimeHours { get; set; } = 4;

    public static string RoleName(RolesNames role)
    {
        return role switch
        {
            RolesNames.Normal => "Normal",
            RolesNames.Seller => "Seller",
            RolesNames.ServiceProvider => "ServiceProvider",
            RolesNames.Manager => "Manager",
            RolesNames.Administrator => "Administrator",
            _ => throw new ArgumentException("Invalid role")
        };
    }
}
public enum RolesNames
{
    Normal,
    Seller,
    ServiceProvider,
    Manager,
    Administrator,
}