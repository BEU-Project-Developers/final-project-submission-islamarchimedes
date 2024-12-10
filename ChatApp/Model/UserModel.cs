using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace ChatApp.Model
{
    public class AppUser : IdentityUser
    {
        public List<ChatModel> Chats { get; set; }
        public string? PofilePicture { get; set; }
        public List<MessageModel> Messages { get; set; }
       

    }

}
