﻿using System.Web.Http;
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
        public async Task<IHttpActionResult> CreateFoodOrderAsync([FromBody] FoodOrderDto order)
        {
            if (string.IsNullOrEmpty(order?.RestaurantId) || !(await DatabaseProvider.GetDatabase().CheckRestaurantIdAsync(order.RestaurantId)))
            {
                return BadRequest();
            }
            await DatabaseProvider.GetDatabase().InsertFoodOrderAsync(order);
            return Created(order.Id.ToString(), order);
        }

        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAsync()
        {
            return Ok(await DatabaseProvider.GetDatabase().GetAllFoodOrdersAsync());
        }
    }
}