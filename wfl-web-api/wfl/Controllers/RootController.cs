using System.Web.Http;
using Symphono.Wfl.Models;

namespace Symphono.Wfl.Controllers
{
    [RoutePrefix("api")]
    public class RootController: ApiController
    {
        [Route("")]
        [HttpGet]
        public IHttpActionResult Get()
        {
            return Ok(new Root());
        }
    }
}