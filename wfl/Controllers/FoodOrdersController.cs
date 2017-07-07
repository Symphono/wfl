using System.Web.Http;
using System.Threading.Tasks;
using Symphono.Wfl.Models;
using Symphono.Wfl.Database;
using Microsoft.Practices.Unity;

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
        public async Task<IHttpActionResult> CreateFoodOrder([FromBody] FoodOrderDto order)
        {
            if (string.IsNullOrEmpty(order?.RestaurantId) || !(await DBManager.CheckRestaurantIdAsync(order.RestaurantId)))
            {
                return BadRequest();
            }
            await DBManager.InsertFoodOrderAsync(order);
            return Created(order.Id.ToString(), order);
        }

        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            return Ok(await DBManager.GetAllFoodOrdersAsync());
        }
    }
}