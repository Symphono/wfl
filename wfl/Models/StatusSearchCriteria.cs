﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace Symphono.Wfl.Models
{
    public class StatusSearchCriteria: ICriteria<FoodOrder>
    {
        public FoodOrder.StatusOptions Status { get; set;}
        public FilterDefinition<FoodOrder> CreateFilter()
        {
            return Builders<FoodOrder>.Filter.Eq(nameof(FoodOrder.Status), this.Status);
        }
    }
}