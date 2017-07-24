﻿using System;
using System.Web.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Symphono.Wfl.Models;
using Symphono.Wfl.Database;

namespace Symphono.Wfl.Controllers
{
    public class MenuSelectionsController : ApiController
    {
        private IDBManager dbManager { get; }
        public MenuSelectionsController(IDBManager dbManager) {
           this.dbManager = dbManager;
        }

        [Route("api/food-order/{foodOrderId}/menu-selection")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAsync([FromUri] string foodOrderId)
        {
            FoodOrder order = await dbManager.GetEntityByIdAsync<FoodOrder>(foodOrderId);
            return Ok(order.MenuSelections);
        }

        [Route("api/menu-selection")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAsync()
        {
            return Ok(await dbManager.GetAllEntitiesAsync<MenuSelection>());
        }

        [Route("api/menu-selection/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetByIdAsync([FromUri] string id)
        {
            return Ok(await dbManager.GetEntityByIdAsync<MenuSelection>(id));
        }

        [Route("api/food-order/{foodOrderId}/menu-selection")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateMenuSelectionAsync(MenuSelectionDto selection, [FromUri] string foodOrderId)
        {
            if (await dbManager.GetEntityByIdAsync<FoodOrder>(foodOrderId) == null || dbManager.GetCreationTime(foodOrderId).Date != DateTime.Now.Date || string.IsNullOrEmpty(selection?.Description) || string.IsNullOrEmpty(selection?.OrdererName))
            {
                return BadRequest();
            }
            MenuSelection s = new MenuSelection()
            {
                OrdererName = selection.OrdererName,
                Description = selection.Description,
                FoodOrderId = foodOrderId
            };
            s = await dbManager.InsertEntityAsync(s);

            FoodOrder order = await dbManager.GetEntityByIdAsync<FoodOrder>(foodOrderId);
            if (order.MenuSelections == null)
            {
                order.MenuSelections = (new[] { s });
            }
            else
                order.MenuSelections = order.MenuSelections.Concat(new[] { s });
            return Ok(await dbManager.UpdateEntityAsync(foodOrderId, order));
        }
    }
}