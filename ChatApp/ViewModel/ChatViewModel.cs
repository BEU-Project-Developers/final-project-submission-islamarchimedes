using ChatApp.Core;
using ChatApp.Dtos;
using ChatApp.Model;
using ChatApp.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ChatApp.ViewModel
{
    public class ChatViewModel : ObservableObject
    {
        // Services
        private readonly ChatService _chatService;
        private readonly MessageService _messageService;
        private readonly AuthService _appUserService;

        // Observable Collections
        public ObservableCollection<ChatModel> Chats { get; set; }
        public ObservableCollection<MessageModel> Messages { get; set; }
        public ObservableCollection<AppUser> Users { get; set; }
        private string _message;

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }

        // Selected Chat
        private ChatModel _selectedChat;
        public ChatModel SelectedChat
        {
            get => _selectedChat;
            set
            {
                _selectedChat = value;
                OnPropertyChanged(nameof(SelectedChat));
                LoadMessagesAsync();
            }
        }

        // Constructor
        public ChatViewModel(ChatService chatService, MessageService messageService, AuthService appUserService)
        {
            _chatService = chatService ?? throw new ArgumentNullException(nameof(chatService));
            _messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
            _appUserService = appUserService ?? throw new ArgumentNullException(nameof(appUserService));

            Chats = new ObservableCollection<ChatModel>();
            Messages = new ObservableCollection<MessageModel>();
            Users = new ObservableCollection<AppUser>();

            // Load Chats
            //LoadChatsAsync();
            LoadUsersAsync();
        }

        private async Task LoadUsersAsync()
        {
            try
            {
                var users = await _appUserService.GetAppUsersAsync();
                Users.Clear();

                foreach (var user in users)
                {
                    Users.Add(user);
                }
            }
            catch (Exception ex)
            {
                // Handle error (e.g., log the exception)
                Console.WriteLine($"Error loading users: {ex.Message}");
            }
        }
        // Load Chats
        private async Task LoadChatsAsync()
        {
            try
            {
                var chats = await _chatService.GetAllChatsAsync(); // Replace with actual user ID
                Chats.Clear();

                foreach (var chat in chats)
                {
                    Chats.Add(chat);
                }
            }
            catch (Exception ex)
            {
                // Handle error (e.g., log the exception)
                Console.WriteLine($"Error loading chats: {ex.Message}");
            }
        }

        // Load Messages for Selected Chat
        private async Task LoadMessagesAsync()
        {
            if (SelectedChat == null) return;

            try
            {
                var messages = await _messageService.GetMessagesAsync(SelectedChat.Id);
                Messages.Clear();

                foreach (var message in messages)
                {
                    Messages.Add(message);
                }
            }
            catch (Exception ex)
            {
                // Handle error
                Console.WriteLine($"Error loading messages: {ex.Message}");
            }
        }

        // Command for Sending Messages
        //private ICommand _sendMessageCommand;
        //public ICommand SendMessageCommand
        //{
        //    get
        //    {
        //        return _sendMessageCommand ??= new RelayCommand(
        //            async (param) => await SendMessageAsync(param as string),
        //            (param) => SelectedChat != null && !string.IsNullOrWhiteSpace(param as string));
        //    }
        //}

        private async Task SendMessageAsync(string messageContent)
        {
            if (SelectedChat == null || string.IsNullOrWhiteSpace(messageContent)) return;

            try
            {
                var message = new CreateMessageDto
                {
                    ChatId = SelectedChat.Id,
                    senderId = "currentUserId", // Replace with actual user ID
                    content = messageContent
                };

                await _messageService.SendMessageAsync(message);

                // Add the new message to the UI
                Messages.Add(new MessageModel
                {
                    ChatId = message.ChatId,
                    senderId = message.senderId,
                    content = message.content,
                    CreatedAt = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                // Handle error
                Console.WriteLine($"Error sending message: {ex.Message}");
            }
        }

        // Property Changed Event
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
