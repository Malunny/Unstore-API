using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Unstore.Controllers;
[Authorize(Roles = "Admin")]
public partial class UserController(IMapper mapper) : ControllerBase
{
    private IMapper _mapper = mapper;
}
