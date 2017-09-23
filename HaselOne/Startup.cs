using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HaselOne.Startup))]
namespace HaselOne
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
            //app.MapSignalR();
        }
    }
}
