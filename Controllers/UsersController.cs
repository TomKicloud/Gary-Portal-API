using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GaryPortalAPI.Models;
using GaryPortalAPI.Services;
using GaryPortalAPI.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GaryPortalAPI.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserService _userRepository;

        public UsersController(IUserService userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        [Produces(typeof(ICollection<User>))]
        public async Task<IActionResult> GetAllUsers(int teamId = 0, CancellationToken ct = default)
        {
            return Ok(await _userRepository.GetAllAsync(ct));
        }

        [HttpGet("{uuid}")]
        [Produces(typeof(User))]
        public async Task<IActionResult> GetUserByUUID(string uuid, CancellationToken ct = default)
        {
            return Ok(await _userRepository.GetByIdAsync(uuid, ct));
        }

        [HttpGet("CurrentUser")]
        [Produces(typeof(User))]
        public async Task<IActionResult> GetCurrentUser(CancellationToken ct = default)
        {
            return Ok(await _userRepository.GetByIdAsync(AuthenticationUtilities.GetUUIDFromIdentity(User), ct));
        }

        [AllowAnonymous]
        [HttpGet("IsUsernameFree/{username}")]
        [Produces(typeof(bool))]
        public async Task<IActionResult> GetIsUsernameFree(string username, CancellationToken ct = default)
        {
            return Ok(await _userRepository.IsUsernameFreeAsync(username, ct));
        }

        [AllowAnonymous]
        [HttpGet("IsEmailFree/{email}")]
        [Produces(typeof(bool))]
        public async Task<IActionResult> GetIsEmailFree(string email, CancellationToken ct = default)
        {
            return Ok(await _userRepository.IsEmailFreeAsync(email, ct));
        }

        [HttpGet("GetPointsForUser/{uuid}")]
        [Produces(typeof(UserPoints))]
        public async Task<IActionResult> GetUserPoints(string uuid, CancellationToken ct = default)
        {
            return Ok(await _userRepository.GetPointsForUserAsync(uuid, ct));
        }

        [HttpPost("ReportUser/{uuid}")]
        public async Task<IActionResult> ReportUser([FromBody] UserReport report, CancellationToken ct = default)
        {
            await _userRepository.ReportUserAsync(report, ct);
            return Ok();
        }

        [HttpPut("UpdatePointsForUser/{uuid}")]
        [Produces(typeof(UserPoints))]
        public async Task<IActionResult> UpdatePointsForUser(string uuid, [FromBody] UserPoints points, CancellationToken ct = default)
        {
            if (!AuthenticationUtilities.IsSameUserOrPrivileged(User, uuid))
                return Unauthorized("You do not have access to this endpoint.");

            return Ok(await _userRepository.UpdatePointsForUserAsync(uuid, points, ct));
        }

        [HttpPut("UpdateDetailsForUser/{uuid}")]
        public async Task<IActionResult> UpdateDetailsForUser(string uuid, [FromBody] UserDetails details, CancellationToken ct = default)
        {
            if (!AuthenticationUtilities.IsSameUserOrPrivileged(User, uuid))
                return Unauthorized("You do not have access to this endpoint");

            return Ok(await _userRepository.UpdateUserDetailsAsync(uuid, details, ct));
        }



        [HttpPost("UpdateProfilePictureForUser/{uuid}")]
        public async Task<IActionResult> UpdateProfilePictureForUser(string uuid, CancellationToken ct = default)
        {
            if (!AuthenticationUtilities.IsSameUserOrAdmin(User, uuid))
                return Unauthorized("You do not have access to this endpoint");

            if (HttpContext.Request.Form.Files.Count > 0)
            {
                return Ok(await _userRepository.UpdateUserProfilePictureAsync(uuid, HttpContext.Request.Form.Files.First(), ct));
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("BlockUser/{uuid}/{blockedUUID}")]
        public async Task<IActionResult> BlockUser(string uuid, string blockedUUID, CancellationToken ct = default)
        {
            return Ok(await _userRepository.BlockUserAsync(uuid, blockedUUID, ct));
        }

        [HttpPost("UnblockUser/{uuid}/{blockedUUID}")]
        public async Task<IActionResult> UnblockUser(string uuid, string blockedUUID, CancellationToken ct = default)
        {
            await _userRepository.UnblockUserAsync(uuid, blockedUUID, ct);
            return Ok();
        }

        [HttpGet("GetBlockedUsersFor/{uuid}")]
        public async Task<IActionResult> GetBlockedUsersFor(string uuid, CancellationToken ct = default)
        {
            return Ok(await _userRepository.GetAllBlocksAsync(uuid, ct));
        }
    }
}
