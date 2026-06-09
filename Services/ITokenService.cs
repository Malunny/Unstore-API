using Unstore.Models;

namespace Unstore.Services;

public interface ITokenService
{
    string GenerateToken(User user);
}