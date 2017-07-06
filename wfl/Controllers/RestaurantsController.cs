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
        IUnityContainer container = new UnityContainer();
        bool isUnityRegistrationComplete = false;

        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateRestaurantAsync([FromBody] RestaurantDto restaurant)
        {
            if (string.IsNullOrEmpty(restaurant?.Name))
            {
                return BadRequest();
            }
            if (!isUnityRegistrationComplete)
            {
                DIContainer.RegisterElements(container);
                isUnityRegistrationComplete = true;
            }
            await container.Resolve<IDBManager>().InsertRestaurantAsync(restaurant);
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
            if (!isUnityRegistrationComplete)
            {
                DIContainer.RegisterElements(container);
                isUnityRegistrationComplete = true;
            }
            return Ok(await container.Resolve<IDBManager>().UpdateRestaurantAsync(id, restaurant));
        }

        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAsync()
        {
            if (!isUnityRegistrationComplete)
            {
                DIContainer.RegisterElements(container);
                isUnityRegistrationComplete = true;
            }
            return Ok(await container.Resolve<IDBManager>().GetAllRestaurantsAsync());
        }

    }
}