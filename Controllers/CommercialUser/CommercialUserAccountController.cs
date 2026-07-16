using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unstore.DTOs;
using Unstore.Services;
using Unstore.Services.Account;
using Unstore.Services.CommercialUser;

namespace Unstore.Controllers.CommercialUser;

[ApiController]
public class CommercialUserAccountController : ControllerBase
{
   [HttpPost("v1/user/commercial")]
   [AllowAnonymous]
   public async Task<IActionResult> CreateCommercialAccount([FromServices] CommercialAccountService service,
      [FromBody] CommercialUserCreateDto dto)
   {
      var username = User.Identity?.Name;

      if (username == null)
         return Unauthorized();

      var serviceResult = await service.TryRegisterCommercialAccountAsync(dto, username);

      return serviceResult.OperationStatus.ToObjectResult(serviceResult.Data);
   }

   [HttpGet("v1/user/commercial")]
   public async Task<IActionResult> GetOwnCommercialAccount([FromServices] CommercialAccountService service)
   {
      var username = User.Identity?.Name;
         
      if (username == null)
         return Unauthorized();
      
      var serviceResult = await service.GetOwnCommercialAccountAsync(username);

      return serviceResult.OperationStatus.ToObjectResult(serviceResult.Data);
   }
}