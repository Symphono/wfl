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
        private IDBManager<FoodOrder> foodOrderDBManager { get; }
        public MenuSelectionsController(IDBManager<FoodOrder> foodOrderDBManager) {
           this.foodOrderDBManager = foodOrderDBManager;
        }

        [Route("{selectionId}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetByIdAsync([FromUri] string foodOrderId, [FromUri] string selectionId)
        {
            FoodOrder order = await foodOrderDBManager.GetEntityByIdAsync(foodOrderId);
            return Ok(order.MenuSelections.FirstOrDefault(x => x.Id == selectionId));
        }

        [Route("{selectionId}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteByIdAsync([FromUri] string foodOrderId, [FromUri] string selectionId)
        {
            FoodOrder order = await foodOrderDBManager.GetEntityByIdAsync(foodOrderId);
            MenuSelection selectionToDelete = order.MenuSelections.FirstOrDefault(x => x.Id == selectionId);
            if (selectionToDelete == null)
            {
                return BadRequest();
            }
            order.MenuSelections.Remove(selectionToDelete);
            await foodOrderDBManager.UpdateEntityAsync(foodOrderId, order);
            return Ok(await foodOrderDBManager.GetEntityByIdAsync(foodOrderId));
        }

        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateMenuSelectionAsync(MenuSelectionDto selection, [FromUri] string foodOrderId)
        {
            FoodOrder order = await foodOrderDBManager.GetEntityByIdAsync(foodOrderId);
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
            return Ok(await foodOrderDBManager.UpdateEntityAsync(foodOrderId, order));
        }
    }
}