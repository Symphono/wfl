using System.Web.Http;
using System.Threading.Tasks;
using Symphono.Wfl.Models;
using Symphono.Wfl.Database;
using Microsoft.Practices.Unity;

namespace Symphono.Wfl.Controllers
{
    [RoutePrefix("api/FoodOrder")]
    public class FoodOrdersController : ApiController
    {
        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateFoodOrder([FromBody] FoodOrderDto order)
        {
            if (string.IsNullOrEmpty(order?.RestaurantId) || !(await DIContainerConfig.GetConfiguredContainer().Resolve<IDBManager>().CheckRestaurantIdAsync(order.RestaurantId)))
            {
                return BadRequest();
            }
            await DIContainerConfig.GetConfiguredContainer().Resolve<IDBManager>().InsertFoodOrderAsync(order);
            return Created(order.Id.ToString(), order);
        }

        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            return Ok(await DIContainerConfig.GetConfiguredContainer().Resolve<IDBManager>().GetAllFoodOrdersAsync());
        }
    }
}