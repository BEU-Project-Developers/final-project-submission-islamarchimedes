using ChatApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Dtos
{
    public class CreateChatDto
    {
        public List<AppUser> Participants { get; set; }
        public bool IsGroup { get; set; }
        public string Name { get; set; }
    }
}
