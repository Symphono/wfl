﻿using Symphono.Wfl.Controllers;
using Microsoft.Practices.Unity;
using Symphono.Wfl.Database;

namespace Symphono.Wfl
{
    public class DIContainerConfig
    {
        public static void RegisterElements(IUnityContainer container)
        {
            container.RegisterType<IDBManager, MongoDBManager>();
            container.RegisterType(typeof(RestaurantsController));
            container.RegisterType(typeof(FoodOrdersController));
        }
    }
}