using System;
using System.Web.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Symphono.Wfl.Models;
using Symphono.Wfl.Database;

namespace Symphono.Wfl.Controllers
{
    [RoutePrefix("api/food-order/{foodOrderid}/menu-selection")]
    public class MenuSelectionsController : ApiController
    {
        private IDBManager dbManager { get; }
        public MenuSelectionsController(IDBManager dbManager) {
           this.dbManager = dbManager;
        }

        [Route("{selectionId}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetByIdAsync([FromUri] string foodOrderId, [FromUri] string selectionId)
        {
            FoodOrder order = await dbManager.GetEntityByIdAsync<FoodOrder>(foodOrderId);
            return Ok(order.MenuSelections.FirstOrDefault(x => x.Id == selectionId));
        }

        [Route("{selectionId}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteByIdAsync([FromUri] string foodOrderId, [FromUri] string selectionId)
        {
            FoodOrder order = await dbManager.GetEntityByIdAsync<FoodOrder>(foodOrderId);
            order.MenuSelections = order.MenuSelections.Where(x => x.Id != selectionId).ToList();
            await dbManager.UpdateEntityAsync(foodOrderId, order);
            return Ok(await dbManager.GetEntityByIdAsync<FoodOrder>(foodOrderId));
        }

        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateMenuSelectionAsync(MenuSelectionDto selection, [FromUri] string foodOrderId)
        {
            FoodOrder order = await dbManager.GetEntityByIdAsync<FoodOrder>(foodOrderId);
            if (order?.Status != FoodOrder.StatusOptions.Active || string.IsNullOrEmpty(selection?.Description) || string.IsNullOrEmpty(selection?.OrdererName))
            {
                return BadRequest();
            }
            MenuSelection selectionEntity = new MenuSelection()
            {
                OrdererName = selection.OrdererName,
                Description = selection.Description,
            };
            selectionEntity.FoodOrder = order;
            order.addMenuSelection(selectionEntity);
            return Ok(await dbManager.UpdateEntityAsync(foodOrderId, order));
        }
    }
}