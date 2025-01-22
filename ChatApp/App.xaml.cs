using System.Windows;
using ChatApp.Repositories;
using ChatApp.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using ChatApp.Model;
using ChatApp.View;
using Microsoft.AspNetCore.Identity;
using ChatApp.ViewModel;
using System;
using WPF_LoginForm.View;

namespace ChatApp
{
    public partial class App : Application
    {
        public static IHost? AppHost { get; private set; }

        public App()
        {
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    // Configure DbContext
                    services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseSqlServer("Server=ASUS-PC\\SQLEXPRESS;Database=myChatDb;Trusted_Connection=True;TrustServerCertificate=True;"));

                    // Configure Identity
                    services.AddIdentityCore<AppUser>(options =>
                    {
                        options.SignIn.RequireConfirmedAccount = false;
                        options.User.RequireUniqueEmail = true;
                        options.Password.RequireDigit = false;
                        options.Password.RequireLowercase = false;
                        options.Password.RequireUppercase = false;
                        options.Password.RequireNonAlphanumeric = false;
                        options.Password.RequiredLength = 4;
                    })
                    .AddEntityFrameworkStores<ApplicationDbContext>();

                    // Add Services
                    services.AddHttpContextAccessor();
                    services.AddTransient<AuthService>();
                    services.AddTransient<ChatService>();
                    services.AddTransient<MessageService>();
                    services.AddScoped<UserManager<AppUser>>();
                    services.AddTransient<ChatViewModel>(); // Register ChatViewModel
                    services.AddTransient<ChatView>(provider =>
                    {
                        var viewModel = provider.GetRequiredService<ChatViewModel>();
                        return new ChatView(viewModel); // Pass ViewModel to ChatView constructor
                    });
                    services.AddTransient<LoginView>();




                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await AppHost!.StartAsync();
            base.OnStartup(e);
            var loginView = AppHost.Services.GetRequiredService<LoginView>();
            loginView.Show(); // Show the ChatView


        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await AppHost!.StopAsync();
            base.OnExit(e);
        }
    }
}
