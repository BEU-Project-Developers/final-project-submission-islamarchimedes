using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Model
{
    public class ChatModel : BaseModel
    {
        public List<AppUser> Participants { get; set; }
        public List<MessageModel> Messages { get; set; }
        public bool IsGroup { get; set; }
        public string Name { get; set; }


    }
}
