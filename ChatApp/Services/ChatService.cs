using ChatApp.Dtos;
using ChatApp.Model;
using ChatApp.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Services
{
    public class ChatService
    {
        private readonly ApplicationDbContext _context;

        public ChatService(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        
        public async Task CreateChatAsync(CreateChatDto chatDto)
        {
            if (chatDto == null) throw new ArgumentNullException(nameof(chatDto));

            try
            {
                var chat = new ChatModel
                {
                    Name = chatDto.IsGroup ? chatDto.Name : chatDto.Participants.FirstOrDefault()?.UserName ?? "Unnamed Chat",
                    IsGroup = chatDto.IsGroup,
                    Participants = chatDto.Participants ?? new List<AppUser>()
                };

                _context.Chats.Add(chat);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                
                throw new InvalidOperationException("Error while creating chat", ex);
            }
        }


        public async Task<List<ChatModel>> GetAllChatsAsync()
        {
            try
            {
                return await _context.Chats
                    .Include(c => c.Participants)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                
                throw new InvalidOperationException("Error while retrieving chats", ex);
            }
        }
       
        public async Task<List<ChatModel>> GetChatsAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId)) throw new ArgumentException("User ID cannot be null or empty", nameof(userId));

            try
            {
                return await _context.Chats
                    .Where(c => c.Participants.Any(p => p.Id == userId))
                    .Include(c => c.Participants)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
              
                throw new InvalidOperationException("Error while retrieving chats", ex);
            }
        }

      
        public async Task<ChatModel> GetChatAsync(int chatId)
        {
            try
            {
                var chat = await _context.Chats
                    .Include(c => c.Participants)
                    .FirstOrDefaultAsync(c => c.Id == chatId);

                if (chat == null) throw new KeyNotFoundException($"Chat with ID {chatId} not found");

                return chat;
            }
            catch (Exception ex)
            {
                
                throw new InvalidOperationException($"Error while retrieving chat with ID {chatId}", ex);
            }
        }

   
        public async Task AddUserToChatAsync(int chatId, AppUser user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            try
            {
                var chat = await _context.Chats
                    .Include(c => c.Participants)
                    .FirstOrDefaultAsync(c => c.Id == chatId);

                if (chat == null) throw new KeyNotFoundException($"Chat with ID {chatId} not found");

                if (chat.Participants.Any(p => p.Id == user.Id))
                    throw new InvalidOperationException("User is already a participant of the chat");
                if(chat.IsGroup == false)
                {
                    throw new InvalidOperationException("Chat is not a group chat");
                }
                chat.Participants.Add(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                
                throw new InvalidOperationException($"Error while adding user to chat with ID {chatId}", ex);
            }
        }

    
        public async Task RemoveUserFromChatAsync(int chatId, AppUser user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            try
            {
                var chat = await _context.Chats
                    .Include(c => c.Participants)
                    .FirstOrDefaultAsync(c => c.Id == chatId);

                if (chat == null) throw new KeyNotFoundException($"Chat with ID {chatId} not found");

                if (!chat.Participants.Any(p => p.Id == user.Id))
                    throw new InvalidOperationException("User is not a participant of the chat");
              if(chat.IsGroup == false)
                {
                    throw new InvalidOperationException("Chat is not a group chat");
                }

                chat.Participants.Remove(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log exception
                throw new InvalidOperationException($"Error while removing user from chat with ID {chatId}", ex);
            }
        }

        // Delete a chat
        public async Task DeleteChatAsync(int chatId)
        {
            try
            {
                var chat = await _context.Chats.FirstOrDefaultAsync(c => c.Id == chatId);

                if (chat == null) throw new KeyNotFoundException($"Chat with ID {chatId} not found");

                _context.Chats.Remove(chat);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log exception
                throw new InvalidOperationException($"Error while deleting chat with ID {chatId}", ex);
            }
        }
    }
}
