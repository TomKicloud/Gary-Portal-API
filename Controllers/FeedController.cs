using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using GaryPortalAPI.Models;
using GaryPortalAPI.Models.Feed;
using GaryPortalAPI.Services;
using GaryPortalAPI.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GaryPortalAPI.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class FeedController : Controller
    {
        private readonly IFeedService _feedService;

        public FeedController(IFeedService feedService)
        {
            _feedService = feedService;
        }

        [HttpGet]
        [Produces(typeof(ICollection<FeedPost>))]
        public async Task<IActionResult> GetFeed(long startfrom, int limit = 10, int teamId = 0, CancellationToken ct = default)
        {
            if (startfrom == 0)
            {
                startfrom = DateTime.UtcNow.Millisecond;
            }
            return Ok(await _feedService.GetAllAsync(startfrom, teamId, limit, ct));
        }

        [HttpGet("{feedPostId}")]
        [Produces(typeof(FeedPost))]
        public async Task<IActionResult> GetFeedPost(int feedPostId, CancellationToken ct = default)
        {
            return Ok(await _feedService.GetByIdAsync(feedPostId, ct));
        }

        [HttpPut("ToggleLike/{feedPostId}/{userUUID}")]
        public async Task<IActionResult> ToggleLikeForPost(int feedPostId, string userUUID, CancellationToken ct = default)
        {
            if (!AuthenticationUtilities.IsSameUser(User, userUUID))
                return Unauthorized("You do not have access to Like for this user");

            await _feedService.ToggleLikeForPostAsync(feedPostId, userUUID, ct);
            return Ok();
        }


        [HttpGet("GetCommentsForPost/{postId}")]
        [Produces(typeof(ICollection<FeedComment>))]
        public async Task<IActionResult> GetCommentsForPost(int postId, CancellationToken ct = default)
        {
            return Ok(await _feedService.GetCommentsForPostAsync(postId, ct));
        }

        [HttpGet("GetComment/{commentId}")]
        [Produces(typeof(FeedComment))]
        public async Task<IActionResult> GetComment(int commentId, CancellationToken ct = default)
        {
            return Ok(await _feedService.GetCommentByIdAsync(commentId, ct));
        }

        [HttpPost("CommentOnPost")]
        public async Task<IActionResult> CommentOnPost([FromBody] FeedComment comment, CancellationToken ct = default)
        {
            if (!AuthenticationUtilities.IsSameUser(User, comment.UserUUID))
                return Unauthorized("You do not have access to comment for this user");

            return Ok(await _feedService.AddCommentToPostAsync(comment, ct));
        }

        [HttpPut("DeleteComment/{commentId}")]
        public async Task<IActionResult> DeleteComment(int commentId, CancellationToken ct = default)
        {
            FeedComment comment = await _feedService.GetCommentByIdAsync(commentId, ct);
            if (!AuthenticationUtilities.IsSameUserOrPrivileged(User, comment.UserUUID))
                return Unauthorized("You do not have access to delete this comment");

            await _feedService.MarkFeedCommentAsDeletedAsync(commentId, ct);
            return Ok();
        }


        [HttpPost("UploadMediaAttachment")]
        public async Task<IActionResult> UploadFeedMediaAttachment()
        {
            if (HttpContext.Request.Form.Files.Count > 0)
                return Ok(await _feedService.UploadMediaAttachmentAsync(HttpContext.Request.Form.Files[0]));
            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewPost([FromBody] FeedPost post, CancellationToken ct = default)
        {
            return Ok(await _feedService.UploadNewPostAsync(post, ct));
        }

        [HttpPut("DeletePost")]
        public async Task<IActionResult> DeletePost(int feedPostId, CancellationToken ct = default)
        {
            FeedPost post = await _feedService.GetByIdAsync(feedPostId);
            if (!AuthenticationUtilities.IsSameUserOrPrivileged(User, post.PosterUUID))
                return Unauthorized("You do not have access to modify this post");

            await _feedService.MarkPostAsDeletedAsync(feedPostId, ct);
            return Ok();
        }

        [HttpPut("VoteFor/{pollAnswerId}/{userUUID}")]
        public async Task<IActionResult> VoteForPoll(int pollAnswerId, string userUUID, bool isVotingFor = true)
        {
            if (!AuthenticationUtilities.IsSameUser(User, userUUID))
                return Unauthorized("You do not have access to vote for this user");
            await _feedService.VoteForPollAsync(userUUID, pollAnswerId, isVotingFor);
            return Ok();
        }

        [HttpPut("ResetVotes/{postId}")]
        public async Task<IActionResult> ResetPollVotes(int postId, CancellationToken ct = default)
        {
            FeedPost post = await _feedService.GetByIdAsync(postId);
            if (!AuthenticationUtilities.IsSameUser(User, post.PosterUUID))
                return Unauthorized("You do not have access to edit this post");
            if (post == null || post.PostType != "poll")
                return BadRequest("Poll does not exist at this id");

            await _feedService.ResetPollVotesAsync(postId, ct);
            return Ok();
        }

        [HttpGet("AditLogs")]
        public async Task<IActionResult> GetAditLogsAsync(int teamId = 0, CancellationToken ct = default)
        {
            return Ok(await _feedService.GetAllAditLogsAsync(teamId, ct));
        }

        [HttpGet("AditLogs/{aditLogId}")]
        public async Task<IActionResult> GetAditLogByIdAsync(int aditLogId, CancellationToken ct = default)
        {
            return Ok(await _feedService.GetAditLogAsync(aditLogId, ct));
        }

        [HttpPost("UploadAditLogAttachment")]
        public async Task<IActionResult> UploadAditLogAttachment(CancellationToken ct = default)
        {
            if (HttpContext.Request.Form.Files.Count >= 2)
            {
                return Ok(await _feedService.UploadAditLogMediaAsync(HttpContext.Request.Form.Files.FirstOrDefault(), HttpContext.Request.Form.Files[1], ct));
            } else if (HttpContext.Request.Form.Files.Count == 1)
            {
                return Ok(await _feedService.UploadAditLogMediaAsync(HttpContext.Request.Form.Files.FirstOrDefault(), ct: ct));
            }

            return BadRequest("No files uploaded");
        }

        [HttpPost("AditLog")]
        public async Task<IActionResult> CreateNewAditLog([FromBody] AditLog aditLog, CancellationToken ct = default)
        {
            return Ok(await _feedService.UploadNewAditLogAsync(aditLog, ct));
        }

        [HttpPut("DeleteAditLog/{aditLogId}")]
        public async Task<IActionResult> DeleteAditLog(int aditLogId, CancellationToken ct = default)
        {
            AditLog aditLog = await _feedService.GetAditLogAsync(aditLogId);
            if (!AuthenticationUtilities.IsSameUserOrPrivileged(User, aditLog.PosterUUID))
                return Unauthorized("You do not have access to modify this Adit Log");

            await _feedService.MarkAditLogAsDeletedAsync(aditLogId, ct);
            return Ok();
        }

        [HttpPut("WatchedAditLog/{aditLogId}/{userUUID}")]
        public async Task<IActionResult> WatchAditLog(int aditLogId, string userUUID, CancellationToken ct = default)
        {
            if (!AuthenticationUtilities.IsSameUser(User, userUUID))
                Unauthorized("You do not have access to mark this adit log as watched");
            await _feedService.WatchAditLogAsync(aditLogId, userUUID, ct);
            return Ok();
        }

        [HttpPost("ReportPost/{postId}")]
        public async Task<IActionResult> ReportUser([FromBody] FeedReport report, CancellationToken ct = default)
        {
            await _feedService.ReportPostAsync(report, ct);
            return Ok();
        }
    }
}
