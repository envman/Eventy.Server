using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using EventServer.Api.Extensions;
using EventServer.Api.Models;

namespace EventServer.Api.Controllers
{
    [Authorize]
    public class EventController : ApiController
    {
        [HttpGet]
        public IEnumerable<EventHeader> Get()
        {
            using (var context = new ApplicationDbContext())
            {
                var user = this.CurrentUser();

                return context.EventUsers
                    .Where(eu => eu.UserId == user.Id)
                    .Select(eu => new EventHeader
                    {
                        Id = eu.EventId,
                        Name = eu.Event.Name,
                        ImageId = eu.Event.ImageId,
                        Description = eu.Event.Description,
                    }).ToList();
            }
        }

        [HttpGet]
        public Event Get([FromUri]Guid id)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Events
                    .Single(e => e.Id == id);
            }
        }

        [HttpPut]
        public void Put([FromUri]Guid id, [FromBody]Event @event)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Events.Add(@event);
                context.EventUsers.Add(new EventUser
                {
                    Id = Guid.NewGuid(),
                    Attending = Attending.Yes,
                    EventId = @event.Id,
                    Owner = true,
                    UserId = this.CurrentUser().Id,
                });

                context.SaveChanges();
            }
        }
    }
}
