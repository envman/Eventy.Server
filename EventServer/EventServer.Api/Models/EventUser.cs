using System;

namespace EventServer.Api.Models
{
    public class EventUser
    {
        public Guid Id { get; set; }

        public bool Owner { get; set; }
        public Attending Attending { get; set; } = Attending.NotSelected;

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public Guid EventId { get; set; }
        public virtual Event Event { get; set; }
    }
}