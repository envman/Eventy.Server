using System;
using System.Linq;
using System.Web.Http;
using EventServer.Api.Models;

namespace EventServer.Api.Controllers
{
    [Authorize]
    public class ScheduleController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Get(Guid id)
        {
            using (var context = new ApplicationDbContext())
            {
                var items = context.ScheduleItems
                    .Where(e => e.EventId == id)
                    .ToList();

                return Json(items);
            }
        }

        [HttpPost]
        public void Post(ScheduleItem item)
        {
            using (var context = new ApplicationDbContext())
            {
                context.ScheduleItems
                    .Add(item);

                context.SaveChanges();
            }
        }
    }
}
