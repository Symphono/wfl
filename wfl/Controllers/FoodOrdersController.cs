﻿using System.Web.Http;
using System.Threading.Tasks;
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
            if (string.IsNullOrEmpty(order?.RestaurantId) || await restaurantDBManager.GetEntityByIdAsync(order.RestaurantId) == null)
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
        public async Task<IHttpActionResult> GetAsync()
        {
            return Ok(await foodOrderDBManager.GetAllEntitiesAsync());
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetByIdAsync([FromUri] string id)
        {
            return Ok(await foodOrderDBManager.GetEntityByIdAsync(id));
        }
    }
}