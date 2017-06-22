using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using MongoDB.Driver;
using wfl.Models;

namespace wfl
{
    public class DBManager
    {
        private static string connectionString;
        public static MongoClient client;
        public static IMongoDatabase db;

        static DBManager()
        {
            connectionString = ConfigurationManager.ConnectionStrings["databaseConnectionString"].ConnectionString;
            client = new MongoClient(connectionString);
            db = client.GetDatabase("WFL");
        }

        public static async Task InsertRestaurantAsync(Restaurant r)
        {
            IMongoCollection<Restaurant> collection = db.GetCollection<Restaurant>("restaurants");
            await collection.InsertOneAsync(r);
        }

        public static async Task<IEnumerable<Restaurant> > GetAllRestaurantsAsync()
        {
            IMongoCollection<Restaurant> collection = db.GetCollection<Restaurant>("restaurants");
            IAsyncCursor<Restaurant> task = await collection.FindAsync(r => true, null);
            return task.ToEnumerable<Restaurant>();
        }
    }
}