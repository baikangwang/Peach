using Microsoft.Owin;

using Peah.YouHu.API;

[assembly: OwinStartup(typeof(Startup))]

namespace Peah.YouHu.API
{
    using Owin;

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
