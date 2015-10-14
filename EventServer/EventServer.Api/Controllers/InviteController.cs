using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;
using EventServer.Api.Models;
using EventServer.Api.Services;
using Microsoft.AspNet.Identity.Owin;
using SendGrid;

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

                EmailInvite(invite);

                context.SaveChanges();
            }
        }

        private void EmailInvite(Invite invite)
        {
            // Create the email object first, then add the properties.
            var myMessage = new SendGridMessage();

            // Add the message properties.
            myMessage.From = new MailAddress("noreply@joinin.com");

            // Add multiple addresses to the To field.
            var recipients = new List<string>
            {
                invite.Email,
            };

            myMessage.AddTo(recipients);

            myMessage.Subject = "Joinin Invite";

            //var link = $"http://joinin.azurewebsites.net/AcceptInvite/{invite.Id}";

            //Add the HTML and Text bodies
            myMessage.Text = "You should join in!";

            var username = "azure_92229888a847e114db064c535f5d6e4b";
            var pswd = "ipxspx123";
            var credentials = new NetworkCredential(username, pswd);

            var transportWeb = new Web(credentials);

            transportWeb.DeliverAsync(myMessage).Wait();
        }
    }
}
