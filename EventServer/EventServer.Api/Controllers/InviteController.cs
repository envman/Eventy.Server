using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using EventServer.Api.Models;
using Microsoft.AspNet.Identity.Owin;

namespace EventServer.Api.Controllers
{
    [Authorize]
    public class InviteController : ApiController
    {
        [HttpPost]
        public void Post(Invite invite)
        {
            using (var context = new ApplicationDbContext())
            {
                string userName = $"User-{Guid.NewGuid()}";
                var user = new ApplicationUser { UserName = userName, Email = invite.Email };

                var result = Request.GetOwinContext()
                    .GetUserManager<ApplicationUserManager>()
                    .CreateAsync(user, Guid.NewGuid().ToString()).Result;

                if (!result.Succeeded)
                {
                    throw new Exception("Error Inviting!");
                }

                var createdUser = context.Users
                    .Single(u => u.UserName == userName);

                invite.CreatedUser = createdUser;

                context.EventUsers.Add(new EventUser
                {
                    EventId = invite.EventId,
                    UserId = createdUser.Id,
                    Owner = false,
                    Attending = Attending.NotSelected,
                });

                context.Invites
                    .Add(invite);

                context.SaveChanges();
            }
        }
    }
}
