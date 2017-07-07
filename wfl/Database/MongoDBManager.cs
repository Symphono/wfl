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

        public async Task<Restaurant> InsertRestaurantAsync(RestaurantDto r)
        {
            IMongoCollection<Restaurant> collection = db.GetCollection<Restaurant>("restaurants");
            Restaurant restaurant = new Restaurant()
            {
                Name = r.Name
            };
            await collection.InsertOneAsync(restaurant);
            return restaurant;
        }

        public async Task<Restaurant> UpdateRestaurantAsync(string id, RestaurantDto r)
        {
            IMongoCollection<Restaurant> collection = db.GetCollection<Restaurant>("restaurants");
            var filter = Builders<Restaurant>.Filter.Eq("Id", id);
            Restaurant restaurant = new Restaurant
            {
                Name = r.Name
            };
            await collection.ReplaceOneAsync(filter, restaurant);
            IAsyncCursor<Restaurant> task = await collection.FindAsync(filter);
            return await task.FirstAsync();
        }

        public async Task<IEnumerable<Restaurant> > GetAllRestaurantsAsync()
        {
            IMongoCollection<Restaurant> collection = db.GetCollection<Restaurant>("restaurants");
            IAsyncCursor<Restaurant> task = await collection.FindAsync(r => true, null);
            return task.ToEnumerable<Restaurant>();
        }

        public async Task<Restaurant> GetRestaurantWithIdAsync(string Id)
        {
            IMongoCollection<Restaurant> collection = db.GetCollection<Restaurant>("restaurants");
            IAsyncCursor<Restaurant> task = await collection.FindAsync(r => r.Id == Id, null);
            return await task.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<FoodOrder>> GetAllFoodOrdersAsync()
        {
            IMongoCollection<FoodOrder> collection = db.GetCollection<FoodOrder>("food-orders");
            IAsyncCursor<FoodOrder> task = await collection.FindAsync(order => true, null);
            return task.ToEnumerable<FoodOrder>();
        }

        public async Task<FoodOrder> InsertFoodOrderAsync(FoodOrderDto o)
        {
            IMongoCollection<FoodOrder> collection = db.GetCollection<FoodOrder>("food-orders");
            FoodOrder order = new FoodOrder
            {
                RestaurantId = o.RestaurantId
            };
            await collection.InsertOneAsync(order);
            return order;
        }
    }
}