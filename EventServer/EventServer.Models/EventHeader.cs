using System;

namespace EventServer.Api.Controllers
{
    public class EventHeader
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}