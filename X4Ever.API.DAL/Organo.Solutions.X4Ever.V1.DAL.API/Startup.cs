using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Organo.Solutions.X4Ever.V1.DAL.API.Startup))]

namespace Organo.Solutions.X4Ever.V1.DAL.API
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}