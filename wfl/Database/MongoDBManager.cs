using System.Collections.Generic;
using System.Web.Configuration;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using Symphono.Wfl.Models;
using System;

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

        private string GenerateCollectionName<T>()
        {
            Type entityType = typeof(T);
            string type = entityType.Name;
            string collectionName = "";
            bool started = false;
            foreach(char c in type)
            {
                if (char.IsUpper(c))
                {
                    if (started == true)
                    {
                        collectionName += '-';
                    }
                    else
                        started = true;
                    collectionName += char.ToLower(c);
                }
                else
                    collectionName += c;
            }
            collectionName += 's';
            return collectionName;

        }

        public async Task<T> InsertEntityAsync<T>(T entity) where T: IEntity
        {
            IMongoCollection<T> collection = db.GetCollection<T>(GenerateCollectionName<T>());
            await collection.InsertOneAsync(entity);
            return entity;
        }

        public async Task<IEnumerable<T>> GetAllEntitiesAsync<T>() where T: IEntity
        {
            IMongoCollection<T> collection = db.GetCollection<T>(GenerateCollectionName<T>());
            IAsyncCursor<T> task = await collection.FindAsync(e => true, null);
            return task.ToEnumerable();
        }

        public async Task<T> GetEntityByIdAsync<T>(string id) where T: IEntity
        {
            IMongoCollection<T> collection = db.GetCollection<T>(GenerateCollectionName<T>());
            var filter = Builders<T>.Filter.Eq("Id", id);
            IAsyncCursor<T> task = await collection.FindAsync(filter, null);
            return await task.FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<T>> GetEntitiesByDateAsync<T>(DateTime date) where T : IEntity
        {
            IMongoCollection<T> collection = db.GetCollection<T>(GenerateCollectionName<T>());
            var lowerBound = Builders<T>.Filter.Gt("Id", new ObjectId(date.Date, 0, 0, 0));
            var upperBound = Builders<T>.Filter.Lt("Id", new ObjectId(date.Date.AddDays(1), 0, 0, 0));
            var bounds = new FilterDefinition<T>[] { lowerBound, upperBound };
            var filter = Builders<T>.Filter.And(bounds);
            
            IAsyncCursor<T> task = await collection.FindAsync(lowerBound, null);
            return task.ToEnumerable();
        }

        public async Task<T> UpdateEntityAsync<T>(string id, T entity) where T : IEntity
        {
            IMongoCollection<T> collection = db.GetCollection<T>(GenerateCollectionName<T>());
            var filter = Builders<T>.Filter.Eq("Id", id);
            await collection.ReplaceOneAsync(filter, entity);
            IAsyncCursor<T> task = await collection.FindAsync(filter);
            return await task.FirstAsync();
        }
    }
}