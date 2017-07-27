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

        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAsync([FromUri] string foodOrderId)
        {
            FoodOrder order = await dbManager.GetEntityByIdAsync<FoodOrder>(foodOrderId);
            return Ok(order.MenuSelections);
        }

        [Route("{selectionId}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetByIdAsync([FromUri] string foodOrderId, [FromUri] string selectionId)
        {
            FoodOrder order = await dbManager.GetEntityByIdAsync<FoodOrder>(foodOrderId);
            return Ok(order.MenuSelections.FirstOrDefault(x => x.Id == selectionId));
        }

        [Route("{index}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteByIdAsync([FromUri] string foodOrderId, [FromUri] int index)
        {
            FoodOrder order = await dbManager.GetEntityByIdAsync<FoodOrder>(foodOrderId);
            order.MenuSelections = order.MenuSelections.Where(x => x.Index != index).ToList();
            await dbManager.UpdateEntityAsync(foodOrderId, order);
            return Ok(await dbManager.GetEntityByIdAsync<FoodOrder>(foodOrderId));
        }

        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateMenuSelectionAsync(MenuSelectionDto selection, [FromUri] string foodOrderId)
        {
            if (await dbManager.GetEntityByIdAsync<FoodOrder>(foodOrderId) == null || dbManager.GetCreationTime(foodOrderId).Date != DateTime.Now.Date || string.IsNullOrEmpty(selection?.Description) || string.IsNullOrEmpty(selection?.OrdererName))
            {
                return BadRequest();
            }
            FoodOrder order = await dbManager.GetEntityByIdAsync<FoodOrder>(foodOrderId);
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