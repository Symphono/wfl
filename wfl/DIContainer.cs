using Microsoft.Practices.Unity;
using Symphono.Wfl.Database;

namespace Symphono.Wfl
{
    public class DIContainer
    {
        public static void RegisterElements(IUnityContainer container)
        {
            container.RegisterType<IDBManager, MongoDBManager>();
        }
    }
}