using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;
using wfl;

[assembly: OwinStartup(typeof(OwinConfiguration))]

namespace wfl
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