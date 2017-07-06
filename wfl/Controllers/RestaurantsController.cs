using System.Web.Http;
using System.Threading.Tasks;
using Symphono.Wfl.Models;
using Symphono.Wfl.Database;
using Microsoft.Practices.Unity;

namespace Symphono.Wfl.Controllers
{
    [RoutePrefix("api/Restaurant")]
    public class RestaurantsController : ApiController
    {
        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateRestaurantAsync([FromBody] RestaurantDto restaurant)
        {
            if (string.IsNullOrEmpty(restaurant?.Name))
            {
                return BadRequest();
            }
            await DIContainerConfig.GetConfiguredContainer().Resolve<IDBManager>().InsertRestaurantAsync(restaurant);
            return Created(restaurant.Id.ToString(), restaurant);
        }

        [Route("{id}")]
        [HttpPut]
        public async Task<IHttpActionResult> UpdateAsync([FromUri] string id, [FromBody] RestaurantDto restaurant)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(restaurant?.Name))
            {
                return BadRequest();
            }
            return Ok(await DIContainerConfig.GetConfiguredContainer().Resolve<IDBManager>().UpdateRestaurantAsync(id, restaurant));
        }

        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAsync()
        {
            return Ok(await DIContainerConfig.GetConfiguredContainer().Resolve<IDBManager>().GetAllRestaurantsAsync());
        }

    }
}