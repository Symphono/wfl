using System.Web.Http;
using Microsoft.Owin.Cors;
using Microsoft.Owin;
using Microsoft.Practices.Unity;
using Unity.WebApi;
using Owin;
using Symphono.Wfl;
using Symphono.Wfl.Controllers;

[assembly: OwinStartup(typeof(OwinConfiguration))]

namespace Symphono.Wfl
{
    public class OwinConfiguration
    {
        public void Configuration(IAppBuilder app)
        {
            IUnityContainer container = new UnityContainer();
            DIContainerConfig.RegisterElements(container);
            HttpConfiguration configuration = new HttpConfiguration
            {
                DependencyResolver = new UnityDependencyResolver(container)
            };
            WebApiConfig.Register(configuration);
            app.UseCors(CorsOptions.AllowAll);
            app.UseWebApi(configuration);
        }
    }
}