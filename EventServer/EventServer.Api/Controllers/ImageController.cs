using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using EventServer.Api.Models;

namespace EventServer.Api.Controllers
{
    public class ImageController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage Get(Guid id)
        {
            using (var context = new ApplicationDbContext())
            {
                var image = context.Images
                    .Single(i => i.Id == id);

                var stream = new MemoryStream(image.Data);
                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StreamContent(stream)
                };
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                return result;
            }
        }

        [HttpPost]
        public Guid Post()
        {
            using (var context = new ApplicationDbContext())
            {
                var requestContent = Request.Content;
                var stream = requestContent.ReadAsStreamAsync().Result;
                var memoryStream = new MemoryStream();
                stream.CopyTo(memoryStream);

                var image = new Image
                {
                    Id = Guid.NewGuid(),
                    Data = memoryStream.ToArray(),
                };
                context.Images.Add(image);

                return image.Id;
            }
        }

        [HttpPut]
        public void Put(Guid id)
        {
            using (var context = new ApplicationDbContext())
            {
                var requestContent = Request.Content;
                var stream = requestContent.ReadAsStreamAsync().Result;
                var memoryStream = new MemoryStream();
                stream.CopyTo(memoryStream);

                context.Images.Add(new Image
                {
                    Id = id,
                    Data = memoryStream.ToArray(),
                });
            }
        }
    }
}
