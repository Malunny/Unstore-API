using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Unstore.Controllers;

[ApiController]
[Authorize(Roles = "ADM, Gerente")]
public partial class ClientController : ControllerBase {}
