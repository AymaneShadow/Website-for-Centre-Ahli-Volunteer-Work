using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Ahli.Startup))]
namespace Ahli
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
