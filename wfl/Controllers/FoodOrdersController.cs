using System.Web.Http;
using System.Threading.Tasks;
using Symphono.Wfl.Models;
using Symphono.Wfl.Database;

namespace Symphono.Wfl.Controllers
{
    [RoutePrefix("api/foodorder")]
    public class FoodOrdersController : ApiController
    {
        IDBManager DBManager { get; }
        public FoodOrdersController(IDBManager dbManager)
        {
            this.DBManager = dbManager;
        }
        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateFoodOrder([FromBody] FoodOrderDto order)
        {
            if (string.IsNullOrEmpty(order?.RestaurantId) || await DBManager.GetRestaurantWithIdAsync(order.RestaurantId) == null)
            {
                return BadRequest();
            }
            FoodOrder o = await DBManager.InsertFoodOrderAsync(order);
            return Created(o.Id.ToString(), o);
        }

        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            return Ok(await DBManager.GetAllFoodOrdersAsync());
        }
    }
}