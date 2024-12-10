using ChatApp.Services;
using ChatApp;
using System;
using System.Windows;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Windows.Media;
using Microsoft.Extensions.DependencyInjection;
using ChatApp.View;

namespace WPF_LoginForm.View
{
    public partial class LoginView : Window
    {
        private readonly AuthService _authService;

        public LoginView()
        {
            InitializeComponent();
            _authService = App.AppHost!.Services.GetRequiredService<AuthService>();
        }

        // Handling dragging the window
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        // Minimize window
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        // Close window
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        // Navigate to Sign Up page
        private void GoToSignUpPage_Click(object sender, RoutedEventArgs e)
        {
            var signUpPage = new SignUpView();
            signUpPage.Show();
            this.Close();
        }

        // Login button click
        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUser.Text;
            string password = txtPass.Password;

            // Clear previous error messages
            ErrorTextBlock.Text = string.Empty;
            ErrorTextBlock.Visibility = Visibility.Hidden;
            btnLogin.IsEnabled = false; // Disable the login button during the login process
            //ProgressBar.Visibility = Visibility.Visible; // Show a loading indicator

            // Basic input validation
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ShowError("Username and password cannot be empty.");
                btnLogin.IsEnabled = true;
                //ProgressBar.Visibility = Visibility.Hidden;
                return;
            }

            try
            {
                // Call the AuthService to perform login
                var result = await _authService.LoginAsync(username, password);

                if (result)
                {
                    // Navigate to the main window on successful login
                    var chatView = App.AppHost!.Services.GetRequiredService<ChatView>();
                    chatView.Show();
                    this.Close();
                }
                else
                {
                    // Handle invalid credentials
                    ShowError("Invalid username or password.");
                }
            }
            catch (Exception ex)
            {
                // Log exception and show user-friendly message
                Console.WriteLine($"Error during login: {ex.Message}");
                ShowError("An unexpected error occurred. Please try again later.");
            }
            finally
            {
                // Re-enable login button and hide loading indicator
                btnLogin.IsEnabled = true;
                //ProgressBar.Visibility = Visibility.Hidden;
            }
        }

        // Method to show error message
        private void ShowError(string message)
        {
            ErrorTextBlock.Text = message;
            ErrorTextBlock.Visibility = Visibility.Visible;
            ErrorTextBlock.Foreground = new SolidColorBrush(Colors.Red);
        }

        // Input validation for username and password
        private void txtUser_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            // Clear error message when user starts typing again
            if (ErrorTextBlock.Visibility == Visibility.Visible)
            {
                ErrorTextBlock.Visibility = Visibility.Hidden;
            }
        }

        private void txtPass_PasswordChanged(object sender, RoutedEventArgs e)
        {
            // Clear error message when user starts typing again
            if (ErrorTextBlock.Visibility == Visibility.Visible)
            {
                ErrorTextBlock.Visibility = Visibility.Hidden;
            }
        }
    }
}
