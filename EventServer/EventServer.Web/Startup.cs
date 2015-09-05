using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EventServer.Web.Startup))]
namespace EventServer.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
