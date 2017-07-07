using System.Web.Http;
using System.Threading.Tasks;
using Symphono.Wfl.Models;
using Symphono.Wfl.Database;


namespace Symphono.Wfl.Controllers
{
    [RoutePrefix("api/FoodOrder")]
    public class FoodOrdersController : ApiController
    {
        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateFoodOrder([FromBody] FoodOrderDto order)
        {
            if (string.IsNullOrEmpty(order?.RestaurantId) || await DatabaseProvider.GetDatabase().GetRestaurantWithIdAsync(order.RestaurantId) == null)
            {
                return BadRequest();
            }
            FoodOrder o = await DatabaseProvider.GetDatabase().InsertFoodOrderAsync(order);
            return Created(o.Id.ToString(), o);
        }

        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            return Ok(await DatabaseProvider.GetDatabase().GetAllFoodOrdersAsync());
        }
    }
}