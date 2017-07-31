using System.Web.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
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
        public async Task<IHttpActionResult> GetAsync([FromUri] StatusSearchCriteria criteria)
        {
            IEnumerable<FoodOrder> orders = await dbManager.GetAllEntitiesAsync<FoodOrder>();
            if (criteria?.Status == EntityStatus.Status.Active || criteria?.Status == EntityStatus.Status.Completed || criteria?.Status == EntityStatus.Status.Discarded)
            {
                orders = orders.Where(o => o.Status == criteria.Status);
            }
            else if (criteria != null)
            {
                return BadRequest();
            }
            FoodOrderCollection foodOrderCollection = new FoodOrderCollection()
            {
                FoodOrders = orders,
                Criteria = criteria
            };
            return Ok(foodOrderCollection);
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetByIdAsync([FromUri] string id)
        {
            return Ok(await dbManager.GetEntityByIdAsync<FoodOrder>(id));
        }

        [Route("{id}/discard")]
        [HttpPost]
        public async Task<IHttpActionResult> DiscardAsync([FromUri] string id)
        {
            FoodOrder order = await dbManager.GetEntityByIdAsync<FoodOrder>(id);
            if (order.Status != EntityStatus.Status.Active)
            {
                return BadRequest();
            }
            order.setStatus(EntityStatus.Status.Discarded);
            return Ok(await dbManager.UpdateEntityAsync(id, order));
        }

        [Route("{id}/reactivate")]
        [HttpPost]
        public async Task<IHttpActionResult> ReactivateAsync([FromUri] string id)
        {
            FoodOrder order = await dbManager.GetEntityByIdAsync<FoodOrder>(id);
            if (order.Status != EntityStatus.Status.Discarded)
            {
                return BadRequest();
            }
            order.setStatus(EntityStatus.Status.Active);
            return Ok(await dbManager.UpdateEntityAsync(id, order));
        }

        [Route("{id}/complete")]
        [HttpPost]
        public async Task<IHttpActionResult> CompleteAsync([FromUri] string id)
        {
            FoodOrder order = await dbManager.GetEntityByIdAsync<FoodOrder>(id);
            if (order.Status != EntityStatus.Status.Active)
            {
                return BadRequest();
            }
            order.setStatus(EntityStatus.Status.Completed);
            return Ok(await dbManager.UpdateEntityAsync(id, order));
        }
    }

}