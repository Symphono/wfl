using System;
using Microsoft.Practices.Unity;
using Symphono.Wfl.Database;

namespace Symphono.Wfl
{
    public class DIContainerConfig
    {
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() => {
            var container = new UnityContainer();
            RegisterElements(container);
            return container;
        });

        public static void RegisterElements(IUnityContainer container)
        {
            container.RegisterType<IDBManager, MongoDBManager>();
        }

        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
    }
}