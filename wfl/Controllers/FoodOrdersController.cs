using System.Web.Http;
using System.Threading.Tasks;
using Symphono.Wfl.Models;
using Symphono.Wfl.Database;

namespace Symphono.Wfl.Controllers
{
    [RoutePrefix("api/food-order")]
    public class FoodOrdersController : ApiController
    {
        private IDBManager dbManager { get; }
        public FoodOrdersController(IDBManager dbManager)
        {
            this.dbManager = dbManager;
        }
        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateFoodOrderAsync([FromBody] FoodOrderDto order)
        {
            if (string.IsNullOrEmpty(order?.RestaurantId) || await dbManager.GetEntityByIdAsync<Restaurant>(order.RestaurantId) == null)
            {
                return BadRequest();
            }
            FoodOrder o = new FoodOrder()
            {
                RestaurantId = order.RestaurantId
            };
            o = await dbManager.InsertEntityAsync(o);
            return Created(o.Id.ToString(), o);
        }

        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAsync()
        {
            return Ok(await dbManager.GetAllEntitiesAsync<FoodOrder>());
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetByIdAsync([FromUri] string id)
        {
            return Ok(await dbManager.GetEntityByIdAsync<FoodOrder>(id));
        }

        [Route("{id}")]
        [HttpPost]
        public async Task<IHttpActionResult> SetStatusAsync([FromUri] string id, [FromBody] FoodOrderStatusDto dto)
        {
            FoodOrder order = await dbManager.GetEntityByIdAsync<FoodOrder>(id);
            if (order?.Status == dto?.Status || order?.Status == FoodOrder.StatusOptions.Completed || (order?.Status == FoodOrder.StatusOptions.Discarded && dto?.Status == FoodOrder.StatusOptions.Completed))
            {
                return BadRequest();
            }
            order.setStatus(dto.Status);
            return Ok(await dbManager.UpdateEntityAsync(id, order));
        }
    }
}