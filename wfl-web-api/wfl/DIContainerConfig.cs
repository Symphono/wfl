using Symphono.Wfl.Controllers;
using Microsoft.Practices.Unity;
using System.Web.Configuration;
using Symphono.Wfl.Database;
using Symphono.Wfl.Models;

namespace Symphono.Wfl
{
    public class DIContainerConfig
    {
        public static void RegisterElements(IUnityContainer container)
        {
            string connectionString = WebConfigurationManager.ConnectionStrings["remoteDBConnectionString"].ConnectionString;
            container.RegisterType<IDBManager<FoodOrder>, MongoDBManager<FoodOrder>>(new InjectionConstructor("food-orders", connectionString));
            container.RegisterType<IDBManager<Restaurant>, MongoDBManager<Restaurant>>(new InjectionConstructor("restaurants", connectionString));
            container.RegisterType(typeof(RestaurantsController));
            container.RegisterType(typeof(FoodOrdersController));
        }
    }
}