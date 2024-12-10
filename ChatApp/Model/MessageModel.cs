using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Model
{
    public class MessageModel : BaseModel
    {
        public int ChatId { get; set; }
        public ChatModel Chat { get; set; }
        public string senderId { get; set; }
        public AppUser sender { get; set; }
        public string content { get; set; }


    }
}
