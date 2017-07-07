using System;
using Microsoft.Practices.Unity;
using Symphono.Wfl.Database;

namespace Symphono.Wfl
{
    public class DIContainerConfig
    {
        public static void RegisterElements(IUnityContainer container)
        {
            container.RegisterType<IDBManager, MongoDBManager>();
        }
    }
}