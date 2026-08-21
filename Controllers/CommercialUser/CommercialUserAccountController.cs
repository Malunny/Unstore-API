using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unstore.DTOs;
using Unstore.Services;
using Unstore.Services.Account;
using Unstore.Services.CommercialUser;

namespace Unstore.Controllers.CommercialUser;

[Authorize]
public class CommercialUserAccountController : UnstoreController
{
   [HttpPost]
   public async Task<IActionResult> Register([FromServices] CommercialAccountService service,
      [FromBody] CommercialUserCreateDto dto)
   {
      var username = User.Identity?.Name;
      
      if (username == null)
         return Unauthorized();

      var serviceResult = await service.TryRegisterCommercialAccountAsync(dto, username);

      return serviceResult.OperationStatus.ToObjectResult(serviceResult.Data);
   }

   [HttpGet]
   [Authorize(Roles = "Seller")]
   public async Task<IActionResult> OwnCommercialInfo([FromServices] CommercialAccountService service)
   {
      var username = User.Identity?.Name;
         
      if (username == null)
         return Unauthorized();
      
      var serviceResult = await service.GetOwnCommercialAccountAsync(username);

      return serviceResult.OperationStatus.ToObjectResult(serviceResult.Data);
   }
}