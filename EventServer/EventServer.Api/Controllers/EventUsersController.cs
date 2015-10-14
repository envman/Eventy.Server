using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using EventServer.Api.Models;

namespace EventServer.Api.Controllers
{
    [RoutePrefix("api/EventUsers")]
    public class EventUsersController : ApiController
    {
        [HttpGet]
        public IEnumerable<UserHeader> Get(Guid id)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.EventUsers
                    .Where(eu => eu.UserId == id.ToString())
                    .Select(eu => new UserHeader
                    {
                        Id = eu.UserId,
                        UserName = eu.User.UserName,
                    }).ToList();
            }
        }

        [HttpGet]
        [Route("Event")]
        public IEnumerable<UserHeader> GetUsersForEvent(Guid eventId)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.EventUsers
                    .Where(eu => eu.EventId == eventId)
                    .Select(eu => new UserHeader
                    {
                        Id = eu.UserId,
                        UserName = eu.User.UserName,
                    }).ToList();
            }
        }

        [HttpPost]
        public void Post(EventUser eventUser)
        {
            using (var context = new ApplicationDbContext())
            {
                if (context.EventUsers.Any(eu => eu.Id == eventUser.Id))
                {
                    context.Entry(eventUser).State = EntityState.Modified;
                }
                else
                {
                    context.EventUsers.Add(eventUser);
                }

                context.SaveChanges();
            }
        }
    }
}
