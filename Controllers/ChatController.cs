﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using GaryPortalAPI.Models;
using GaryPortalAPI.Models.Chat;
using GaryPortalAPI.Services;
using GaryPortalAPI.Services.Authentication;
using GaryPortalAPI.Utilities;
using Microsoft.AspNetCore.Mvc;
using User = GaryPortalAPI.Models.User;

namespace GaryPortalAPI.Controllers
{
    [Route("api/[controller]")]
    public class ChatController : Controller
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpGet("Chats/{userUUID}")]
        [Produces(typeof(ICollection<Chat>))]
        public async Task<IActionResult> GetChatsForUser(string userUUID, CancellationToken ct = default)
        {
            return Ok(await _chatService.GetAllChatsForUserAsync(userUUID, ct));
        }

        [HttpGet("{chatUUID}")]
        [Produces(typeof(Chat))]
        public async Task<IActionResult> GetChatById(string chatUUID, CancellationToken ct = default)
        {
            return Ok(await _chatService.GetChatByIdAsync(chatUUID, ct));
        }

        [HttpGet("Messages/{chatUUID}")]
        [Produces(typeof(ICollection<ChatMessage>))]
        public async Task<IActionResult> GetMessagesForChat(string chatUUID, long startfrom, int limit = 20, CancellationToken ct = default)
        {
            return Ok(await _chatService.GetMessagesForChatAsync(chatUUID, startfrom, limit, ct));
        }

        [HttpGet("Message/{messageUUID}")]
        [Produces(typeof(ChatMessage))]
        public async Task<IActionResult> GetMessageById(string messageUUID, CancellationToken ct = default)
        {
            return Ok(await _chatService.GetMessageByIdAsync(messageUUID, ct));
        }

        [HttpPut("Chats/AddUser/{userUUID}/{chatUUID}")]
        [Produces(typeof(Chat))]
        public async Task<IActionResult> AddUserToChat(string userUUID, string chatUUID, CancellationToken ct = default)
        {
            return Ok(await _chatService.AddUserToChatAsync(userUUID, chatUUID, ct));
        }

        [HttpPut("Chats/RemoveUser/{userUUID}/{chatUUID}")]
        public async Task<IActionResult> RemoveUserFromChat(string userUUID, string chatUUID, CancellationToken ct = default)
        {
            await _chatService.RemoveFromChatAsync(userUUID, chatUUID, ct);
            return Ok();
        }

        [HttpPost("{chatUUID}/NewMessage")]
        [Produces(typeof(ChatMessage))]
        public async Task<IActionResult> AddMessageToChat(string chatUUID, [FromBody] ChatMessage message, CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid message details");

            return Ok(await _chatService.AddMessageToChatAsync(message, chatUUID, ct));
        }

        [HttpPut("Messages/Delete/{messageUUID}")]
        public async Task<IActionResult> DeleteMessage(string messageUUID, CancellationToken ct = default)
        {
            return Ok(await _chatService.RemoveMessageAsync(messageUUID, ct));
        }

        [HttpPost("{chatUUID}/Attachment")]
        [Produces(typeof(string))]
        public async Task<IActionResult> UploadChatAttachment(string chatUUID, CancellationToken ct = default)
        {
            if (HttpContext.Request.Form.Files.Count > 0)
                return Ok(await _chatService.UploadChatAttachmentAsync(HttpContext.Request.Form.Files[0], chatUUID, ct));

            return BadRequest("No files specified");
        }

        [HttpPost]
        [Produces(typeof(Chat))]
        public async Task<IActionResult> CreateChat([FromBody] Chat chat, CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid chat object");

            return Ok(await _chatService.CreateNewChatAsync(chat, ct));
        }

        [HttpPut]
        [Produces(typeof(Chat))]
        public async Task<IActionResult> EditChat([FromBody] ChatEditDetails details, CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid chat details");

            return Ok(await _chatService.EditChatAsync(details, ct));
        }


        [HttpPut("Chats/{chatUUID}/MarkAsRead")]
        public async Task<IActionResult> MarkChatAsRead(string chatUUID, CancellationToken ct = default)
        {
            await _chatService.MarkChatAsReadAsync(AuthenticationUtilities.GetUUIDFromIdentity(User), chatUUID, ct);
            return Ok();
        }

    }
}