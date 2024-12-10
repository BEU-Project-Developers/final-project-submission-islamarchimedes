using ChatApp;
using ChatApp.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace WPF_LoginForm.View
{
    public partial class SignUpView : Window
    {
        private readonly AuthService _authService;

        public SignUpView()
        {
            InitializeComponent();
            _authService = App.AppHost!.Services.GetRequiredService<AuthService>();
        }

        // Close button click event
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // Minimize button click event
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        
        private void GoToLoginPage_Click(object sender, RoutedEventArgs e)
        {
            var loginPage = new LoginView();
            loginPage.Show();
            this.Close();
        }

        
        private async void btnSignUp_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUser.Text;
            string email = txtEmail.Text;
            string password = txtPass.Password;
            string confirmPassword = txtConfirmPass.Password;

            ErrorTextBlock.Text = string.Empty;
            ErrorTextBlock.Visibility = Visibility.Hidden;

            // Disable the sign-up button and show loading indicator
            btnSignUp.IsEnabled = false;
            //ProgressBar.Visibility = Visibility.Visible;

            // Basic validation
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                ShowError("Please fill in all fields.");
             
                return;
            }

            if (password != confirmPassword)
            {
                ShowError("Passwords do not match.");
              
                return;
            }

            try
            {
                // Attempt registration
                var result = await _authService.RegisterAsync(username, email, password);

                if (result.IsSuccessful)
                {
                    // Show success message
                    MessageBox.Show("Registration successful. Please log in.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    new LoginView().Show();
                    this.Close();
                }
                else
                {
                    // Display the error messages from IdentityResult
                    ShowError(result.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Error during registration: {ex.Message}");
                ShowError("An unexpected error occurred. Please try again later.");
            }
            finally
            {
                
                btnSignUp.IsEnabled = true;
               
            }
        }

        // Method to show error message
        private void ShowError(string message)
        {
            ErrorTextBlock.Text = message;
            ErrorTextBlock.Visibility = Visibility.Visible;
            ErrorTextBlock.Foreground = new SolidColorBrush(Colors.Red);
            btnSignUp.IsEnabled = true;
        }

       
        private void txtUser_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (ErrorTextBlock.Visibility == Visibility.Visible)
            {
                ErrorTextBlock.Visibility = Visibility.Hidden;
            }
        }

        private void txtEmail_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (ErrorTextBlock.Visibility == Visibility.Visible)
            {
                ErrorTextBlock.Visibility = Visibility.Hidden;
            }
        }

        private void txtPass_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (ErrorTextBlock.Visibility == Visibility.Visible)
            {
                ErrorTextBlock.Visibility = Visibility.Hidden;
            }
        }

        private void txtConfirmPass_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (ErrorTextBlock.Visibility == Visibility.Visible)
            {
                ErrorTextBlock.Visibility = Visibility.Hidden;
            }
        }
    }
}
