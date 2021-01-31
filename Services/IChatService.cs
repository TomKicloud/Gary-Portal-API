using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GaryPortalAPI.Models;
using GaryPortalAPI.Models.Chat;
using GaryPortalAPI.Services.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace GaryPortalAPI.Services
{
    public interface IChatService : IDisposable
    {
        Task<Chat> CreateNewChatAsync(Chat chat, CancellationToken ct = default);
        Task<Chat> GetChatByIdAsync(string uuid, CancellationToken ct = default);
        Task<Chat> EditChatAsync(ChatEditDetails newChat, CancellationToken ct = default);
        Task<ICollection<Chat>> GetAllChatsForUserAsync(string userUUID, CancellationToken ct = default);
        Task<ICollection<ChatMember>> GetMembersForChatAsync(string chatUUID, CancellationToken ct = default);

        Task<ChatMessage> GetLastMessageForChat(string chatUUID, CancellationToken ct = default);
        Task<ICollection<ChatMessage>> GetMessagesForChatAsync(string chatUUID, long startfrom, int limit = 20, CancellationToken ct = default);
        Task<ChatMessage> GetMessageByIdAsync(string messageUUID, CancellationToken ct = default);
        Task MarkChatAsReadAsync(string userUUID, string chatUUID, CancellationToken ct = default);

        Task<Chat> AddUserToChatAsync(string userUUID, string chatUUID, CancellationToken ct = default);
        Task RemoveFromChatAsync(string userUUID, string chatUUID, CancellationToken ct = default);

        Task<ChatMessage> AddMessageToChatAsync(ChatMessage msg, string chatUUID, CancellationToken ct = default);
        Task<bool> RemoveMessageAsync(string messageUUID, CancellationToken ct = default);
        Task<string> UploadChatAttachmentAsync(IFormFile file, string chatUUID, CancellationToken ct = default);
    }

    public class ChatService : IChatService
    {

        private readonly AppDbContext _context;

        public ChatService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Chat> CreateNewChatAsync(Chat chat, CancellationToken ct = default)
        {
            chat.ChatUUID = Guid.NewGuid().ToString("N");
            await _context.Chats.AddAsync(chat, ct);
            await _context.SaveChangesAsync(ct);
            return chat;
        }

        public async Task<Chat> GetChatByIdAsync(string uuid, CancellationToken ct = default)
        {
            Chat chat = await _context.Chats
                .AsNoTracking()
                .Include(c => c.ChatMembers.Where(cm => cm.IsInChat))
                .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(c => c.ChatUUID == uuid, cancellationToken: ct);
            chat.LastChatMessage = await GetLastMessageForChat(chat.ChatUUID, ct);

            foreach (ChatMember member in chat.ChatMembers)
            {
                member.UserDTO = member.User.ConvertToDTO();
                member.User = null;
            }
            return chat;
        }

        public async Task<Chat> EditChatAsync(ChatEditDetails newChat, CancellationToken ct = default)
        {
            Chat chat = await _context.Chats.AsNoTracking().FirstOrDefaultAsync(c => c.ChatUUID == newChat.ChatUUID, cancellationToken: ct);
            if (chat == null)
                return null;

            chat.ChatName = newChat.ChatName;
            chat.ChatIsProtected = newChat.ChatIsProtected;
            chat.ChatIsDeleted = newChat.ChatIsDeleted;
            chat.ChatIsPublic = newChat.ChatIsPublic;
            _context.Update(chat);
            await _context.SaveChangesAsync(ct);
            return await GetChatByIdAsync(newChat.ChatUUID, ct);
        }

        public async Task<ICollection<Chat>> GetAllChatsForUserAsync(string userUUID, CancellationToken ct = default)
        {
            ICollection<Chat> chats = await _context.Chats
                .AsNoTracking()
                .Include(c => c.ChatMembers.Where(cm => cm.IsInChat))
                    .ThenInclude(cm => cm.User)
                .Where(c => c.ChatMembers.Any(cm => cm.UserUUID == userUUID && cm.IsInChat))
                .ToListAsync(ct);

            foreach (Chat chat in chats)
            {
                chat.LastChatMessage = await GetLastMessageForChat(chat.ChatUUID, ct);
                foreach (ChatMember member in chat.ChatMembers)
                {
                    member.UserDTO = member.User.ConvertToDTO();
                    member.User = null;
                }
            }

            return chats.OrderByDescending(c => c.LastChatMessage != null ? c.LastChatMessage.MessageCreatedAt : c.ChatCreatedAt).ToList();
        }



        public async Task<ICollection<ChatMember>> GetMembersForChatAsync(string chatUUID, CancellationToken ct = default)
        {
            return await _context.ChatMembers
                .AsNoTracking()
                .Where(cm => cm.ChatUUID == chatUUID && cm.IsInChat)
                .ToListAsync();
        }



        public async Task<ChatMessage> GetLastMessageForChat(string chatUUID, CancellationToken ct = default)
        {
            return await _context.ChatMessages
                .AsNoTracking()
                .Where(cm => cm.ChatUUID == chatUUID && !cm.MessageIsDeleted)
                .OrderByDescending(cm => cm.MessageCreatedAt)
                .Select(cm => new ChatMessage{ MessageContent = cm.MessageContent, MessageCreatedAt = cm.MessageCreatedAt })
                .FirstOrDefaultAsync(ct);
        }

        public async Task<ChatMessage> GetMessageByIdAsync(string messageUUID, CancellationToken ct = default)
        {
            ChatMessage msg = await _context.ChatMessages
                .AsNoTracking()
                .Include(cm => cm.User)
                .FirstOrDefaultAsync(cm => cm.ChatMessageUUID == messageUUID, cancellationToken: ct);
            msg.UserDTO = msg.User.ConvertToDTO();
            msg.User = null;
            return msg;
        }

        public async Task MarkChatAsReadAsync(string userUUID, string chatUUID, CancellationToken ct = default)
        {
            ChatMember member = await _context.ChatMembers.AsNoTracking().FirstOrDefaultAsync(cm => cm.UserUUID == userUUID && cm.ChatUUID == chatUUID && cm.IsInChat, ct);
            if (member == null)
                return;
            member.LastReadAt = DateTime.UtcNow;
            _context.Update(member);
            await _context.SaveChangesAsync(ct);
        }

        public async Task<ICollection<ChatMessage>> GetMessagesForChatAsync(string chatUUID, long startfrom, int limit = 20, CancellationToken ct = default)
        {
            DateTime fromDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(startfrom);
            ICollection<ChatMessage> chatMessages = await _context.ChatMessages
                .AsNoTracking()
                .Include(cm => cm.User)
                .Where(cm => cm.ChatUUID == chatUUID && !cm.MessageIsDeleted && fromDate >= cm.MessageCreatedAt)
                .OrderByDescending(cm => cm.MessageCreatedAt)
                .Take(limit)
                .ToListAsync(ct);

            foreach (ChatMessage msg in chatMessages)
            {
                msg.UserDTO = msg.User.ConvertToDTO();
                msg.User = null;
            }
            return chatMessages;
        }



        public async Task<Chat> AddUserToChatAsync(string userUUID, string chatUUID, CancellationToken ct = default)
        {
            ChatMember member = new ChatMember
            {
                ChatUUID = chatUUID,
                UserUUID = userUUID,
                IsInChat = true
            };
            ChatMember existingMember = await _context.ChatMembers.AsNoTracking().FirstOrDefaultAsync(cm => cm.UserUUID == userUUID && cm.ChatUUID == chatUUID);
            if (existingMember != null)
            {
                member.IsInChat = true;
                _context.Update(member);
            } else
            {
                await _context.ChatMembers.AddAsync(member, ct);
            }
            await _context.SaveChangesAsync(ct);
            return await GetChatByIdAsync(chatUUID, ct);
        }

        public async Task RemoveFromChatAsync(string userUUID, string chatUUID, CancellationToken ct = default)
        {
            ChatMember member = await _context.ChatMembers.AsNoTracking().FirstOrDefaultAsync(cm => cm.UserUUID == userUUID && cm.ChatUUID == chatUUID, ct);
            if (member == null)
                return;
            member.IsInChat = false;
            _context.Update(member);
            await _context.SaveChangesAsync(ct);
        }




        public async Task<ChatMessage> AddMessageToChatAsync(ChatMessage msg, string chatUUID, CancellationToken ct = default)
        {
            msg.ChatMessageUUID = Guid.NewGuid().ToString("N");
            await _context.ChatMessages.AddAsync(msg, ct);
            await _context.SaveChangesAsync(ct);
            return msg;
        }

        public async Task<bool> RemoveMessageAsync(string messageUUID, CancellationToken ct = default)
        {
            ChatMessage message = await _context.ChatMessages.AsNoTracking().FirstOrDefaultAsync(cm => cm.ChatMessageUUID == messageUUID, ct);
            if (message == null)
                return false;
            message.MessageIsDeleted = true;
            _context.Update(message);
            await _context.SaveChangesAsync(ct);
            return true;
        }

        public async Task<string> UploadChatAttachmentAsync(IFormFile file, string chatUUID, CancellationToken ct = default)
        {
            if (file == null) return null;

            string uuid = Guid.NewGuid().ToString();
            Directory.CreateDirectory($"/var/www/cdn/GaryPortal/Chat/{chatUUID}/Attachments/");
            string newFileName = file.FileName.Replace(Path.GetFileNameWithoutExtension(file.FileName), uuid);
            var filePath = $"/var/www/cdn/GaryPortal/Chat/{chatUUID}/Attachments/{newFileName}";
            using (var stream = new FileStream(filePath, FileMode.Create))
                await file.CopyToAsync(stream, ct);
            return $"https://cdn.tomk.online/GaryPortal/Chat/{chatUUID}/Attachments/{newFileName}";
        }





        public async void Dispose()
        {
            await _context.DisposeAsync();
        }
    }
}
