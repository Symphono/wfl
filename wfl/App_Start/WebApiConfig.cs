using System.Web.Http;
using Drum;
using Hypermedia;
using Hypermedia.Transforms;
using Hypermedia.Siren;
using Symphono.Wfl.Models;

namespace Symphono.Wfl
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.EnableHypermedia()
                .RegisterDefaultTransform(new ReflectiveTransform());

            config.RegisterHypermediaProfiles(typeof(Root).Assembly);
            config.Formatters.Insert(0, new SirenFormatter());
        }
    }
}
