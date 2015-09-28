using System;

namespace EventServer.Api.Models
{
    public class ScheduleItem
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public Event Event { get; set; }
        public DateTime DateTime { get; set; }
        public string Name { get; set; }
    }
}
