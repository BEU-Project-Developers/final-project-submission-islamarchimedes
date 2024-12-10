// ApplicationDbContext.cs
using ChatApp.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace ChatApp.Repositories
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
       public DbSet<ChatModel> Chats { get; set; }
     public   DbSet<MessageModel> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Define the many-to-many relationship between ChatModel and AppUser
            builder.Entity<ChatModel>()
                .HasMany(c => c.Participants)
                .WithMany(u => u.Chats)
                .UsingEntity<Dictionary<string, object>>(
                    "ChatUser", // Join table name
                    j => j.HasOne<AppUser>().WithMany().HasForeignKey("AppUserId"), // Foreign key to AppUser
                    j => j.HasOne<ChatModel>().WithMany().HasForeignKey("ChatModelId") // Foreign key to ChatModel
                );
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
    }


}
