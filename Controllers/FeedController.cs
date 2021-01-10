using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GaryPortalAPI.Models.Feed;
using GaryPortalAPI.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GaryPortalAPI.Controllers
{
    
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
        public async Task<IActionResult> GetFeed(int startfrom, int limit = 10, CancellationToken ct = default)
        {
            return Ok(await _feedService.GetAllAsync(startfrom = DateTime.UtcNow.Millisecond, limit, ct));
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
            await _feedService.ToggleLikeForPostAsync(feedPostId, userUUID, ct);
            return Ok();
        }


        [HttpPost("UploadMediaAttachment")]
        public async Task<IActionResult> UploadFeedMediaAttachment()
        {
            if (HttpContext.Request.Form.Files.Count > 0)
                return Ok(await _feedService.UploadMediaAttachment(HttpContext.Request.Form.Files[0]));
            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewPost([FromBody] FeedPost post)
        {
            //TODO: Upload post
            return Ok();
        }
    }
}
