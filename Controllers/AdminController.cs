using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GaryPortalAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GaryPortalAPI.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
    [Route("api/[controller]")]
    public class AdminController : Controller
    {

        private readonly IUserService _userService;

        public AdminController(IUserService userService)
        {
            _userService = userService;
        }
    

        [HttpPut("ClearAllPrayers")]
        public async Task<IActionResult> AdminClearAllPrayers(string uuid, CancellationToken ct = default)
        {
            if (!string.IsNullOrWhiteSpace(uuid))
            {
                await _userService.ClearPrayersForUserAsync(uuid, ct);
            }
            else
            {
                await _userService.ClearAllPrayersAsync(ct);
            }
            return Ok();

        }

    }
}
