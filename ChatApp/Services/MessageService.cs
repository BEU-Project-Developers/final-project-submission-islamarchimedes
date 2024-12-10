using ChatApp.Dtos;
using ChatApp.Model;
using ChatApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Services
{
    public class MessageService
    {
        private readonly ApplicationDbContext _context;

        public MessageService(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // Send a new message
        public async Task SendMessageAsync(CreateMessageDto messageDto)
        {
            if (messageDto == null) throw new ArgumentNullException(nameof(messageDto));

            try
            {
                var message = new MessageModel
                {
                    ChatId = messageDto.ChatId,
                    senderId = messageDto.senderId,
                    content = messageDto.content,
                    CreatedAt = DateTime.UtcNow
                };

                await _context.Messages.AddAsync(message);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log the error (e.g., using a logger)
                throw new InvalidOperationException("Error while sending message", ex);
            }
        }

        // Retrieve all messages for a specific chat
        public async Task<List<MessageModel>> GetMessagesAsync(int chatId)
        {
            try
            {
                return await Task.Run(() =>
                    _context.Messages
                    .Where(m => m.ChatId == chatId)
                    .OrderBy(m => m.CreatedAt)
                    .ToList());
            }
            catch (Exception ex)
            {
                // Log the error
                throw new InvalidOperationException($"Error while retrieving messages for chat {chatId}", ex);
            }
        }

        // Retrieve a single message by ID
        public async Task<MessageModel> GetMessageAsync(int messageId)
        {
            try
            {
                var message = await Task.Run(() =>
                    _context.Messages.FirstOrDefault(m => m.Id == messageId));

                if (message == null) throw new KeyNotFoundException($"Message with ID {messageId} not found");

                return message;
            }
            catch (Exception ex)
            {
                // Log the error
                throw new InvalidOperationException($"Error while retrieving message with ID {messageId}", ex);
            }
        }

        // Delete a message
        public async Task DeleteMessageAsync(int messageId)
        {
            try
            {
                var message = await GetMessageAsync(messageId);

                if (message == null) throw new KeyNotFoundException($"Message with ID {messageId} not found");

                _context.Messages.Remove(message);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log the error
                throw new InvalidOperationException($"Error while deleting message with ID {messageId}", ex);
            }
        }

        // Update an existing message
        public async Task UpdateMessageAsync(int messageId, CreateMessageDto messageDto)
        {
            if (messageDto == null) throw new ArgumentNullException(nameof(messageDto));

            try
            {
                var message = await GetMessageAsync(messageId);

                if (message == null) throw new KeyNotFoundException($"Message with ID {messageId} not found");

                message.content = messageDto.content;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log the error
                throw new InvalidOperationException($"Error while updating message with ID {messageId}", ex);
            }
        }
    }
}
