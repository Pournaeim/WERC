using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WERC.Startup))]
namespace WERC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
