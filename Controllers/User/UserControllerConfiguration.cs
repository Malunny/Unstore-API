using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Unstore.Controllers;

[ApiController]
[Authorize(Roles = "ADM, Gerente")]
public partial class UserController {}
