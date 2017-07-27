using Symphono.Wfl.Controllers;
using Microsoft.Practices.Unity;
using Symphono.Wfl.Database;
using Symphono.Wfl.Models;

namespace Symphono.Wfl
{
    public class DIContainerConfig
    {
        public static void RegisterElements(IUnityContainer container)
        {
            container.RegisterType<IDBManager<FoodOrder>, MongoDBManager<FoodOrder>>(new InjectionConstructor("food-orders"));
            container.RegisterType<IDBManager<Restaurant>, MongoDBManager<Restaurant>>(new InjectionConstructor("restaurants"));
            container.RegisterType(typeof(RestaurantsController));
            container.RegisterType(typeof(FoodOrdersController));
        }
    }
}