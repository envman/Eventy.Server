using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EventServer.Api.Extensions;
using EventServer.Api.Models;

namespace EventServer.Api.Controllers
{
    [Authorize]
    public class ChatController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Get(Guid id)
        {
            var user = this.CurrentUser();

            using (var context = new ApplicationDbContext())
            {
                if (!context.EventUsers.Any(eu => eu.UserId == user.Id && eu.EventId == id))
                {
                    throw new HttpResponseException(HttpStatusCode.Unauthorized);
                }

                return Json(context.ChatMessages
                    .Where(c => c.EventId == id)
                    .OrderBy(c => c.PostTime)
                    .Select(c => RenderChatMessage(c))
                    .ToList());

            }
        }

        [HttpGet]
        public IHttpActionResult Get(Guid eventId, Guid lastChatId)
        {
            var user = this.CurrentUser();

            using (var context = new ApplicationDbContext())
            {
                if (!context.EventUsers.Any(eu => eu.UserId == user.Id && eu.EventId == eventId))
                {
                    throw new HttpResponseException(HttpStatusCode.Unauthorized);
                }

                var date = context.ChatMessages
                    .Single(c => c.Id == lastChatId)
                    .PostTime;

                return Json(context.ChatMessages
                    .Where(c => c.EventId == eventId)
                    .Where(c => c.PostTime > date)
                    .OrderBy(c => c.PostTime)
                    .Select(c => RenderChatMessage(c))
                    .ToList());
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

        private object RenderChatMessage(ChatMessage message)
        {
            return new
            {
                message.Id,
                message.Message,
                message.PostTime,
                message.Poster.UserName,
            };
        }
    }

    public class ChatPost
    {
        public Guid EventId { get; set; }
        public string Message { get; set; }
    }
}
