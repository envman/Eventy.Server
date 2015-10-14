using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using EventServer.Api.Extensions;
using EventServer.Api.Models;

namespace EventServer.Api.Controllers
{
    [Authorize]
    public class EventUsersController : ApiController
    {
        [HttpGet]
        public IEnumerable<UserHeader> Get()
        {
            using (var context = new ApplicationDbContext())
            {
                var user = this.CurrentUser();

                return context.EventUsers
                    .Where(eu => eu.UserId == user.Id)
                    .Select(eu => new UserHeader
                    {
                        Id = eu.UserId,
                        UserName = eu.User.UserName,
                    }).ToList();
            }
        }

        [HttpGet]
        public IHttpActionResult Get(Guid id)
        {
            using (var context = new ApplicationDbContext())
            {
                var user = this.CurrentUser();

                if (!context.EventUsers.Any(eu => eu.UserId == user.Id && eu.EventId == id))
                {
                    return BadRequest("No No!");
                }

                return Json(context.EventUsers
                    .Where(eu => eu.EventId == id)
                    .Select(eu => new UserHeader
                    {
                        Id = eu.UserId,
                        UserName = eu.User.UserName,
                    }).ToList());
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
