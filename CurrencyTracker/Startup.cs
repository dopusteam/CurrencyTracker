using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CurrencyTracker.Startup))]
namespace CurrencyTracker
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
