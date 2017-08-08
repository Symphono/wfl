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
        private IDBManager<FoodOrder> foodOrderDBManager { get; }
        private IDBManager<Restaurant> restaurantDBManager { get; }
        public FoodOrdersController(IDBManager<FoodOrder> foodOrderDBManager, IDBManager<Restaurant> restaurantDBManager)
        {
            this.foodOrderDBManager = foodOrderDBManager;
            this.restaurantDBManager = restaurantDBManager;
        }
        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateFoodOrderAsync([FromBody] FoodOrderDto order)
        {
            if (order == null || !(await order.CanCreateFoodOrderAsync(restaurantDBManager)))
            {
                return BadRequest();
            }
            FoodOrder o = new FoodOrder()
            {
                RestaurantId = order.RestaurantId
            };
            o = await foodOrderDBManager.InsertEntityAsync(o);
            return Created(o.Id.ToString(), o);
        }

        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAsync([FromUri] StatusSearchCriteria criteria)
        {
            IEnumerable<FoodOrder> orders;
            if (criteria?.Status == FoodOrder.StatusOptions.Active || criteria?.Status == FoodOrder.StatusOptions.Completed || criteria?.Status == FoodOrder.StatusOptions.Discarded)
            {
                orders = await foodOrderDBManager.GetEntitiesAsync(criteria);
            }
            else if (criteria != null)
            {
                return BadRequest();
            }
            else
            {
                orders = await foodOrderDBManager.GetEntitiesAsync(null);
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
            return Ok(await foodOrderDBManager.GetEntityByIdAsync(id));
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
                Values = enumValues
            };
            return Ok(options);
        }

        [Route("{id}")]
        [HttpPost]
        public async Task<IHttpActionResult> SetStatusAsync([FromUri] string id, [FromBody] FoodOrderStatusDto dto)
        {
            FoodOrder order = await foodOrderDBManager.GetEntityByIdAsync(id);
            if (dto == null || !(dto.CanSetStatus(order.Status)))
            {
                return BadRequest();
            }
            order.SetStatus((FoodOrder.StatusOptions) Enum.Parse(typeof(FoodOrder.StatusOptions), dto.Status));
            return Ok(await foodOrderDBManager.UpdateEntityAsync(id, order));
        }
    }
}