
namespace Symphono.Wfl.Database
{
    public class DatabaseProvider
    {
        public static IDBManager GetDatabase()
        {
            return new MongoDBManager();
        }
    }
}