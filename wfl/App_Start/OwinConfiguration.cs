using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;
using Symphono.Wfl;

[assembly: OwinStartup(typeof(OwinConfiguration))]

namespace Symphono.Wfl
{
    public class OwinConfiguration
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration configuration = new HttpConfiguration();
            WebApiConfig.Register(configuration);

            app.UseCors(CorsOptions.AllowAll);
            app.UseWebApi(configuration);
        }
    }
}