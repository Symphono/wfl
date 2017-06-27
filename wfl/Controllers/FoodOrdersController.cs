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
            if (order == null || order.RestaurantId == null || !(await DatabaseProvider.GetDatabase().CheckRestaurantIDAsync(order.RestaurantId)))
            {
                return BadRequest();
            }
            await DatabaseProvider.GetDatabase().InsertFoodOrderAsync(order);
            return Created(order.Id, order);
        }

        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            return Ok(await DatabaseProvider.GetDatabase().GetAllFoodOrdersAsync());
        }
    }
}