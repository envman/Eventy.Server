using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventServer.Api.Models
{
    public class ChatMessage
    {
        public Guid Id { get; set; }
        public string PosterId { get; set; }
        public ApplicationUser Poster { get; set; }
        public Guid EventId { get; set; }
        public Event Event { get; set; }
        public string Message { get; set; }
        public DateTime PostTime { get; set; }
    }
}
