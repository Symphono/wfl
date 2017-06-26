using System.Collections.Generic;
using System.Web.Http.Description;
using System.Web.Http;
using System.Threading.Tasks;
using MongoDB.Bson;
using wfl.Models;


namespace wfl.Controllers
{
    [RoutePrefix("api/Restaurant")]
    public class RestaurantsController : ApiController
    {
        [Route("")]
        [HttpPost]
        [ResponseType(typeof(Restaurant))]
        public async Task<IHttpActionResult> CreateRestaurantAsync([FromBody] Restaurant restaurant)
        {
            if (restaurant == null || restaurant.Name == null || restaurant.Name.Equals(""))
            {
                return BadRequest();
            }
            await DBManager.InsertRestaurantAsync(restaurant);
            return Created(restaurant.ID.ToString(), restaurant);
        }

        [Route("{id}")]
        [HttpPut]
        [ResponseType(typeof(Restaurant))]
        public async Task<IHttpActionResult> UpdateNameAsync([FromUri] string id, [FromBody] Restaurant restaurant)
        {
            if (id == null || id.Equals("") || restaurant == null || restaurant.Name == null || restaurant.Name.Equals(""))
            {
                return BadRequest();
            }
            return Ok(await DBManager.UpdateRestaurantNameAsync(ObjectId.Parse(id), restaurant.Name));
        }

        [Route("")]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<Restaurant>))]
        public async Task<IHttpActionResult> GetAsync()
        {
            return Ok(await DBManager.GetAllRestaurantsAsync());
        }

    }
}