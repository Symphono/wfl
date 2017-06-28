using System.Collections.Generic;
using System.Web.Configuration;
using System.Threading.Tasks;
using MongoDB.Driver;
using Symphono.Wfl.Models;

namespace Symphono.Wfl.Database
{
    public class MongoDBManager: IDBManager
    {
        private static string connectionString;
        public static MongoClient client;
        public static IMongoDatabase db;
        
        public MongoDBManager()
        {
            connectionString = WebConfigurationManager.ConnectionStrings["databaseConnectionString"].ConnectionString;
            client = new MongoClient(connectionString);
            db = client.GetDatabase("WFL");
        }

        public async Task InsertRestaurantAsync(RestaurantDto r)
        {
            r.Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString();
            IMongoCollection<RestaurantDto> collection = db.GetCollection<RestaurantDto>("restaurants");
            await collection.InsertOneAsync(r);
        }

        public async Task<IEnumerable<RestaurantDto> > GetAllRestaurantsAsync()
        {
            IMongoCollection<RestaurantDto> collection = db.GetCollection<RestaurantDto>("restaurants");
            IAsyncCursor<RestaurantDto> task = await collection.FindAsync(r => true, null);
            return task.ToEnumerable<RestaurantDto>();
        }
    }
}