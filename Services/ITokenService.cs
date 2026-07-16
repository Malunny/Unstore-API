using Unstore.Models;

namespace Unstore.Services;

public interface ITokenService
{
    string GenerateToken(Models.User user);
}