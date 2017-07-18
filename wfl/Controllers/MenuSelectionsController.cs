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
        IDBManager DBManager;
        public MenuSelectionsController(IDBManager dbManager) {
           this.DBManager = dbManager;
        }

        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAsync()
        {
            MenuSelectionCollection menuSelectionCollection = new MenuSelectionCollection()
            {
                MenuSelections = await DBManager.GetAllEntitiesAsync<MenuSelection>()
            };
            return Ok(menuSelectionCollection);
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetByIdAsync([FromUri] string id)
        {
            return Ok(await DBManager.GetEntityByIdAsync<MenuSelection>(id));
        }

        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateMenuSelectionAsync(MenuSelectionDto selection)
        {
            IEnumerable<FoodOrder> todaysOrders = await DBManager.GetEntitiesByDateAsync<FoodOrder>(DateTime.Now);
            if (todaysOrders.Any() == false || string.IsNullOrEmpty(selection?.Description) || string.IsNullOrEmpty(selection?.FoodOrderId) || await DBManager.GetEntityByIdAsync<FoodOrder>(selection.FoodOrderId) == null)
            {
                return BadRequest();
            }
            MenuSelection s = new MenuSelection()
            {
                OrdererName = selection.OrdererName,
                Description = selection.Description,
                FoodOrderId = selection.FoodOrderId
            };
            s = await DBManager.InsertEntityAsync(s);
            return Created(s.Id.ToString(), s);
        }
    }
}