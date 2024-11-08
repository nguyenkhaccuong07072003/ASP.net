using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Advanced.Startup))]
namespace Advanced
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
