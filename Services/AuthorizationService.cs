using System.Security.Claims;
using AutoMapper;
using Unstore.Data;

namespace Unstore.Services;

public class AuthorizationService : BaseService
{
    public AuthorizationService(AppDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }

    public bool IsManager(ClaimsPrincipal user)
    {
        var userRole = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
        return userRole == "Manager";
    }
    public bool IsManagerOrAdmin(ClaimsPrincipal user)
    {
        var userRole = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
        return userRole == "Manager" | userRole == "Admin";
    }
}