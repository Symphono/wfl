using System;
using System.Web.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Symphono.Wfl.Models;
using Symphono.Wfl.Database;

namespace Symphono.Wfl.Controllers
{
    [RoutePrefix("api/menu-selection")]
    public class MenuSelectionsController : ApiController
    {
        IDBManager dbManager { get; }
        public MenuSelectionsController(IDBManager dbManager) {
           this.dbManager = dbManager;
        }

        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAsync()
        {
            MenuSelectionCollection menuSelectionCollection = new MenuSelectionCollection()
            {
                MenuSelections = await dbManager.GetAllEntitiesAsync<MenuSelection>()
            };
            return Ok(menuSelectionCollection);
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetByIdAsync([FromUri] string id)
        {
            return Ok(await dbManager.GetEntityByIdAsync<MenuSelection>(id));
        }

        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateMenuSelectionAsync(MenuSelectionDto selection)
        {
            IEnumerable<FoodOrder> todaysOrders = await dbManager.GetEntitiesByDateAsync<FoodOrder>(DateTime.Now);
            if (todaysOrders.Any() == false || string.IsNullOrEmpty(selection?.Description) || string.IsNullOrEmpty(selection?.FoodOrderId) || await dbManager.GetEntityByIdAsync<FoodOrder>(selection.FoodOrderId) == null)
            {
                return BadRequest();
            }
            MenuSelection s = new MenuSelection()
            {
                OrdererName = selection.OrdererName,
                Description = selection.Description,
                FoodOrderId = selection.FoodOrderId
            };
            s = await dbManager.InsertEntityAsync(s);
            return Created(s.Id.ToString(), s);
        }
    }
}