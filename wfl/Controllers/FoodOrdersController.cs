using System.Web.Http;
using System.Threading.Tasks;
using Symphono.Wfl.Models;
using Symphono.Wfl.Database;

namespace Symphono.Wfl.Controllers
{
    [RoutePrefix("api/FoodOrder")]
    public class FoodOrdersController : ApiController
    {
        IDBManager DBManager { get; }
        public FoodOrdersController(IDBManager dbManager)
        {
            this.DBManager = dbManager;
        }
        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateFoodOrderAsync([FromBody] FoodOrderDto order)
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
        public async Task<IHttpActionResult> GetAsync()
        {
            return Ok(await DBManager.GetAllFoodOrdersAsync());
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetByIdAsync([FromUri] string id)
        {
            return Ok(await DBManager.GetFoodOrderWithIdAsync(id));
        }
    }
}