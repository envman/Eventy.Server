using System.Web;
using System.Web.Http;
using EventServer.Api.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace EventServer.Api.Extensions
{
    public static class ApiControllerExtensions
    {
        public static ApplicationUser CurrentUser(this ApiController controller)
        {
            var httpContext = HttpContext.Current;
            var userId = httpContext.User.Identity.GetUserId();
            return httpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(userId);
        }
    }
}
