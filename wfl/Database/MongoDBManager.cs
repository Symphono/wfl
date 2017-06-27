using System.Collections.Generic;
using System.Web.Configuration;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
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

        public async Task<RestaurantDto> UpdateRestaurantNameAsync(string id, string name)
        {
            IMongoCollection<RestaurantDto> collection = db.GetCollection<RestaurantDto>("restaurants");
            var filter = Builders<RestaurantDto>.Filter.Eq("Id", id);
            var update = Builders<RestaurantDto>.Update.Set("Name", name);
            await collection.UpdateOneAsync(filter, update);
            IAsyncCursor<RestaurantDto> task = await collection.FindAsync(filter);
            return task.FirstOrDefault();
        }

        public async Task<IEnumerable<RestaurantDto> > GetAllRestaurantsAsync()
        {
            IMongoCollection<RestaurantDto> collection = db.GetCollection<RestaurantDto>("restaurants");
            IAsyncCursor<RestaurantDto> task = await collection.FindAsync(r => true, null);
            return task.ToEnumerable<RestaurantDto>();
        }
    }
}