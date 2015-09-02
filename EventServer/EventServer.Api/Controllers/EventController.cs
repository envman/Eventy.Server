using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EventServer.Api.Controllers
{
    [Authorize]
    public class EventController : ApiController
    {
        [HttpGet]
        public IEnumerable<Event> Get()
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public Event Get([FromUri]Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
