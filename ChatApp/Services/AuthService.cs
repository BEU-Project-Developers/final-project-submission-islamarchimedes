using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ChatApp.Model;
using Microsoft.EntityFrameworkCore;
using ChatApp.Repositories;
using Microsoft.AspNetCore.Http;




namespace ChatApp.Services
{
    public class AuthService
    {
     
        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public static AppUser CurrentUser { get; private set; }

        public AuthService(UserManager<AppUser> userManager, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager), "UserManager cannot be null.");
            _context = context ?? throw new ArgumentNullException();
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException();
        }

     
        public async Task<AppUser> GetCurrentUserAsync()
        {
            // HttpContext vasitəsilə daxil olan istifadəçi məlumatını alırıq
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

            if (user != null)
            {
                return user;
            }

            return null;
        }
        public async Task<ICollection<AppUser>> GetAppUsersAsync()
        {
            try
            {
                
                return await _userManager.Users.ToListAsync();
            }
            catch (Exception ex)
            {
                
                Console.Error.WriteLine($"Error retrieving users: {ex.Message}");

              
                return new List<AppUser>();
            }
        }


        // Custom Login method with enhanced error handling
        public async Task<bool> LoginAsync(string username, string password)
        {
            try
            {
                // Find user by username
                var user = await _userManager.FindByNameAsync(username);
                if (user == null)
                {
                    // User not found
                    return false;
                }

                // Verify password
                var passwordValid = await _userManager.CheckPasswordAsync(user, password);
                if (!passwordValid)
                {
                    // Password incorrect
                    return false;
                }

                // Successful login
                CurrentUser = user;
                return true;
            }
            catch (ArgumentNullException ex)
            {
                // Handle specific ArgumentNullException for bad parameters
                Console.WriteLine($"Error during login: {ex.Message}");
                // Optionally, log the error to a logging service
                return false;
            }
            catch (Exception ex)
            {
                // Handle any other general exceptions
                Console.WriteLine($"An error occurred during login: {ex.Message}");
                // Optionally, log the error to a logging service
                return false;
            }
        }

        // Custom registration method with enhanced error handling
        public async Task<(bool IsSuccessful, string ErrorMessage)> RegisterAsync(string username, string email, string password)
        {
            try
            {
                var user = new AppUser { UserName = username, Email = email };

                // Attempt to create the user
                var result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    // Registration succeeded
                    var existingUsers = _userManager.Users.Where(u => u.Id != user.Id).ToList();

                    // Create chat records between the new user and each existing user
                    foreach (var existingUser in existingUsers)
                    {
                        var chat = new ChatModel
                        {
                            Participants = new List<AppUser> { user, existingUser },
                            Messages = new List<MessageModel>(), // Initialize an empty message list
                            IsGroup = false, // Single chat between two users
                            Name = $"{user.UserName} & {existingUser.UserName}", // Optional: Name the chat
                            CreatedAt = DateTime.UtcNow // Assuming BaseModel has CreatedAt
                        };

                        // Save the chat to the database
                        _context.Chats.Add(chat);
                    };

                    // Save changes to the database
                    await _context.SaveChangesAsync();

                    return (true, null);
                }
                else
                {
                    // Registration failed, return the error message(s)
                    return (false, string.Join("; ", result.Errors.Select(e => e.Description)));
                }
            }
            catch (ArgumentException ex)
            {
                // Handle invalid argument exceptions, such as invalid email format
                Console.WriteLine($"Argument error during registration: {ex.Message}");
                return (false, "Invalid input.");
            }
            catch (Exception ex)
            {
                // General error handling for unexpected exceptions
                Console.WriteLine($"An error occurred during registration: {ex.Message}");
                return (false, "An unexpected error occurred.");
            }
        }

        // Logout - in WPF, this might be as simple as clearing any session tokens or flags
        public async Task LogoutAsync()
        {
            try
            {
                // Handle logout logic if needed (e.g., clearing session tokens)
                // For WPF applications, you would typically clear local session state
                // Example: Clear any user data from local storage
                Console.WriteLine("User logged out successfully.");
            }
            catch (Exception ex)
            {
                // Handle unexpected errors during logout
                Console.WriteLine($"An error occurred during logout: {ex.Message}");
                // Optionally, log the error
            }

            await Task.CompletedTask; // Complete the async method
        }
    }
}
