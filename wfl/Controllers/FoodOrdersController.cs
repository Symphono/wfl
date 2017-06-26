using System.Collections.Generic;
using System.Web.Http.Description;
using System.Web.Http;
using System.Threading.Tasks;
using wfl.Models;
using MongoDB.Bson;


namespace wfl.Controllers
{
    [RoutePrefix("api/FoodOrder")]
    public class FoodOrdersController : ApiController
    {
        [Route("")]
        [HttpPost]
        [ResponseType(typeof(FoodOrder))]
        public async Task<IHttpActionResult> CreateFoodOrder([FromBody] FoodOrder order)
        {
            if (order == null || order.RestaurantID == null || !(await DBManager.CheckRestaurantIDAsync(ObjectId.Parse(order.RestaurantID))))
            {
                return BadRequest();
            }
            await DBManager.InsertFoodOrderAsync(order);
            return Created(order.ID.ToString(), order);
        }
    }
}