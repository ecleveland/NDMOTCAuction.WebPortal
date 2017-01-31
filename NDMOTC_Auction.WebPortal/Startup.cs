using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NDMOTC_Auction.WebPortal.Startup))]
namespace NDMOTC_Auction.WebPortal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
