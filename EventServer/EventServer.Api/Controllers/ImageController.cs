using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using EventServer.Api.Models;
using ImageResizer;

namespace EventServer.Api.Controllers
{
    public class ImageController : ApiController
    {
        [HttpGet]
        [Authorize]
        public IHttpActionResult Get()
        {
            using (var context = new ApplicationDbContext())
            {
                return Json(context.Images
                    .Select(i => new
                    {
                        i.Id,
                    })
                    .ToList());
            }
        }

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

        [HttpGet]
        public HttpResponseMessage Get(Guid imageId, int width, int height)
        {
            using (var context = new ApplicationDbContext())
            {
                var image = context.Images
                    .Single(i => i.Id == imageId);

                var settings = new ResizeSettings
                {
                    Format = "jpg",
                    Mode = FitMode.Stretch,
                    Width = width,
                    Height = height,
                    Scale = ScaleMode.Both
                };

                var stream = new MemoryStream();
                ImageBuilder.Current.Build(image.Data, stream, settings);

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
                context.SaveChanges();

                return image.Id;
            }
        }

        [HttpPut]
        public void Put(Guid id)
        {
            using (var context = new ApplicationDbContext())
            {
                var requestContent = Request.Content;
                var bytes = requestContent.ReadAsByteArrayAsync().Result;

                if (bytes.Length < 1)
                {
                    throw new Exception("BAD HARRY!");
                }

                context.Images.Add(new Image
                {
                    Id = id,
                    Data = bytes,
                });
                context.SaveChanges();
            }
        }
    }
}
