﻿using Symphono.Wfl.Controllers;
using Microsoft.Practices.Unity;
using System.Configuration;
using Symphono.Wfl.Database;
using Symphono.Wfl.Models;

namespace Symphono.Wfl
{
    public class DIContainerConfig
    {
        public static void RegisterElements(IUnityContainer container)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["databaseConnectionString"].ConnectionString;
            container.RegisterType<IDBManager<FoodOrder>, MongoDBManager<FoodOrder>>(new InjectionConstructor("food-orders", connectionString));
            container.RegisterType<IDBManager<Restaurant>, MongoDBManager<Restaurant>>(new InjectionConstructor("restaurants", connectionString));
            container.RegisterType(typeof(RestaurantsController));
            container.RegisterType(typeof(FoodOrdersController));
        }
    }
}