using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Dtos
{
    public class CreateMessageDto
    {
        public int ChatId { get; set; }
        public string senderId { get; set; }
        public string content { get; set; }
    }
}
