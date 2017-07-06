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
            IMongoCollection<RestaurantDto> collection = db.GetCollection<RestaurantDto>("restaurants");
            await collection.InsertOneAsync(r);
        }

        public async Task<RestaurantDto> UpdateRestaurantAsync(string id, RestaurantDto restaurant)
        {
            IMongoCollection<RestaurantDto> collection = db.GetCollection<RestaurantDto>("restaurants");
            var filter = Builders<RestaurantDto>.Filter.Eq("Id", id);
            await collection.ReplaceOneAsync(filter, restaurant);
            IAsyncCursor<RestaurantDto> task = await collection.FindAsync(filter);
            return await task.FirstAsync();
        }

        public async Task<IEnumerable<RestaurantDto> > GetAllRestaurantsAsync()
        {
            IMongoCollection<RestaurantDto> collection = db.GetCollection<RestaurantDto>("restaurants");
            IAsyncCursor<RestaurantDto> task = await collection.FindAsync(r => true, null);
            return task.ToEnumerable<RestaurantDto>();
        }

        public async Task<IEnumerable<FoodOrderDto>> GetAllFoodOrdersAsync()
        {
            IMongoCollection<FoodOrderDto> collection = db.GetCollection<FoodOrderDto>("food-orders");
            IAsyncCursor<FoodOrderDto> task = await collection.FindAsync(order => true, null);
            return task.ToEnumerable<FoodOrderDto>();
        }

        public async Task InsertFoodOrderAsync(FoodOrderDto order)
        {
            IMongoCollection<FoodOrderDto> collection = db.GetCollection<FoodOrderDto>("food-orders");
            await collection.InsertOneAsync(order);
        }

        public async Task<bool> CheckRestaurantIdAsync(string id)
        {
            IMongoCollection<RestaurantDto> collection = db.GetCollection<RestaurantDto>("restaurants");
            var filter = Builders<RestaurantDto>.Filter.Eq("Id", id);
            IAsyncCursor<RestaurantDto> task = await collection.FindAsync(filter);
            IList<RestaurantDto> queryResults = await task.ToListAsync();
            if (queryResults.Count == 0)
            {
                return false;
            }
            return true;
        }
    }
}