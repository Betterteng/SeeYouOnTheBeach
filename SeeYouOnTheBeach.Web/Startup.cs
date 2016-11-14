using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SeeYouOnTheBeach.Web.Startup))]
namespace SeeYouOnTheBeach.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
