using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using GaryPortalAPI.Models;
using GaryPortalAPI.Services;
using GaryPortalAPI.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GaryPortalAPI.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin,staff")]
    [Route("api/[controller]")]
    public class StaffController : Controller
    {
        private readonly IUserService _userService;
        private readonly IFeedService _feedService;
        private readonly IStaffService _staffService;

        public StaffController(IUserService userService, IFeedService feedService, IStaffService staffService)
        {
            _userService = userService;
            _feedService = feedService;
            _staffService = staffService;
        }

        [HttpPost("BanUser")]
        public async Task<IActionResult> BanUser([FromBody] UserBan ban, CancellationToken ct = default)
        {
            Console.WriteLine($"LOG: User {ban.UserUUID} banned by {ban.BannedByUUID}");
            return Ok(await _userService.BanUserAsync(ban, ct));
        }

        [HttpPut("RevokeBan/{userBanId}")]
        public async Task<IActionResult> RevokeBan(int userBanId, CancellationToken ct = default)
        {
            Console.WriteLine($"LOG: Ban ID {userBanId} revoked by {User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value}");
            await _userService.RevokeBanAsync(userBanId, ct);
            return Ok();
        }

        [HttpPut("MarkUserReportAsDeleted/{reportId}")]
        public async Task<IActionResult> MarkUserReportAsDeleted(int reportId, CancellationToken ct = default)
        {
            await _userService.MarkReportAsDeletedAsync(reportId, ct);
            return Ok();
        }

        [HttpPut("MarkFeedReportAsDeleted/{reportId}")]
        public async Task<IActionResult> MarkFeedReportAsDeleted(int reportId, CancellationToken ct = default)
        {
            await _feedService.MarkReportAsDeletedAsync(reportId, ct);
            return Ok();
        }

        [HttpGet("GetStaffRoomAnnouncements")]
        [Produces(typeof(ICollection<StaffRoomAnnouncement>))]
        public async Task<IActionResult> GetStaffRoomAnnouncements(CancellationToken ct = default)
        {
            return Ok(await _staffService.GetStaffRoomAnnouncementsAsync(ct));
        }

        [HttpGet("GetTeams")]
        [Produces(typeof(ICollection<Team>))]
        public async Task<IActionResult> GetAllTeams(CancellationToken ct = default)
        {
            return Ok(await _staffService.GetAllTeams(ct));
        }
    }
}
