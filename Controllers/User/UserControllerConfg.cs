using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Unstore.Controllers.User;

[ApiController]
[Authorize(Roles = "Administrator")]
public partial class UserController : ControllerBase
{
    
}