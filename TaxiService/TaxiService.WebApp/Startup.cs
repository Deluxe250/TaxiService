using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TaxiService.WebApp.Startup))]
namespace TaxiService.WebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
