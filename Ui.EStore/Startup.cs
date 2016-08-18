using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Ui.EStore.Startup))]
namespace Ui.EStore
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
