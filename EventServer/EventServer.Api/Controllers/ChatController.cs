using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using EventServer.Api.Extensions;
using EventServer.Api.Models;

namespace EventServer.Api.Controllers
{
    [Authorize]
    public class ChatController : ApiController
    {
        [HttpGet]
        public IEnumerable<ChatMessage> Get(Guid eventId)
        {
            var user = this.CurrentUser();

            using (var context = new ApplicationDbContext())
            {
                if (!context.EventUsers.Any(eu => eu.UserId == user.Id && eu.EventId == eventId))
                {
                    throw new HttpResponseException(HttpStatusCode.Unauthorized);
                }

                return context.ChatMessages
                    .Where(c => c.EventId == eventId)
                    .OrderBy(c => c.PostTime)
                    .ToList();
            }
        }

        [HttpPost]
        public Guid Post([FromBody]ChatPost post)
        {
            var user = this.CurrentUser();

            using (var context = new ApplicationDbContext())
            {
                if (!context.EventUsers.Any(eu => eu.UserId == user.Id && eu.EventId == post.EventId))
                {
                    throw new HttpResponseException(HttpStatusCode.Unauthorized);
                }

                var chatMessage = new ChatMessage
                {
                    Id = Guid.NewGuid(),
                    EventId = post.EventId,
                    Message = post.Message,
                    PosterId = user.Id,
                    PostTime = DateTime.Now,
                };
                context.ChatMessages.Add(chatMessage);

                context.SaveChanges();
                return chatMessage.Id;
            }
        }
    }

    public class ChatPost
    {
        public Guid EventId { get; set; }
        public string Message { get; set; }
    }
}
