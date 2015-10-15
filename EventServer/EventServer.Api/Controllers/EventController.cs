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

        [HttpDelete]
        public IHttpActionResult Delete(Guid id)
        {
            using (var context = new ApplicationDbContext())
            {
                var userId = this.CurrentUser().Id;

                if (!context.EventUsers.Any(eu => eu.UserId == userId && eu.EventId == id && eu.Owner))
                {
                    return Unauthorized();
                }

                var existing = context.Events
                    .SingleOrDefault(e => e.Id == id);

                context.Events.Remove(existing);
                context.SaveChanges();

                return Ok();
            }
        }

        [HttpPut]
        public void Put([FromUri]Guid id, [FromBody]Event @event)
        {
            using (var context = new ApplicationDbContext())
            {
                var existing = context.Events
                    .SingleOrDefault(e => e.Id == id);

                if (existing != null)
                {
                    existing.Description = @event.Description;
                    existing.EndDateTime = @event.EndDateTime;
                    existing.ImageId = @event.ImageId;
                    existing.Location = @event.Location;
                    existing.Name = @event.Name;
                    existing.StartDateTime = @event.StartDateTime;
                }
                else
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
                }

                context.SaveChanges();
            }
        }
    }
}
