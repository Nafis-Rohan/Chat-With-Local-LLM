using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BLL.DTOs
{
    public class ChatMessageDTO
    {
        public int Id { get; set; }

        public string Role { get; set; }      // "user" / "assistant"

        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}

