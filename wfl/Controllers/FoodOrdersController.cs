using System;
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
            IEnumerable<FoodOrder> orders;
            if (criteria?.Status == FoodOrder.StatusOptions.Active || criteria?.Status == FoodOrder.StatusOptions.Completed || criteria?.Status == FoodOrder.StatusOptions.Discarded)
            {
                orders = await dbManager.GetFilteredEntities(criteria);
            }
            else if (criteria != null)
            {
                return BadRequest();
            }
            else
            {
                orders = await dbManager.GetAllEntitiesAsync<FoodOrder>();
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

        [Route("status-options")]
        [HttpGet]
        public IHttpActionResult GetStatusOptions()
        {
            IList<string> enumValues = new List<string>();
            foreach(var value in Enum.GetValues(typeof(FoodOrder.StatusOptions)))
            {
                enumValues.Add(value.ToString());
            }
            StatusOptionsRepresentation options = new StatusOptionsRepresentation()
            {
                values = enumValues
            };
            return Ok(options);
        }

        [Route("{id}")]
        [HttpPost]
        public async Task<IHttpActionResult> SetStatusAsync([FromUri] string id, [FromBody] FoodOrderStatusDto dto)
        {
            FoodOrder order = await dbManager.GetEntityByIdAsync<FoodOrder>(id);
            FoodOrder.StatusOptions status;
            if (!Enum.TryParse(dto?.Status, false, out status) || order?.Status == status || order?.Status == FoodOrder.StatusOptions.Completed || (order?.Status == FoodOrder.StatusOptions.Discarded && status == FoodOrder.StatusOptions.Completed))
            {
                return BadRequest();
            }
            order.setStatus(status);
            return Ok(await dbManager.UpdateEntityAsync(id, order));
        }
    }
}