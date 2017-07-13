using System.Collections.Generic;
using System.Web.Configuration;
using System.Threading.Tasks;
using MongoDB.Driver;
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

        private string GenerateCollectionName<T>() where T: new()
        {
            T t = new T();
            string type = t.GetType().ToString();
            string collectionName = "";
            bool started = true;
            foreach(char c in type)
            {
                if(c == '.')
                {
                    collectionName = "";
                    started = false;
                }
                else if (char.IsUpper(c))
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
            Console.WriteLine(collectionName);
            return collectionName;

        }

        public async Task<T> InsertEntityAsync<T>(T entity) where T: IEntity, new()
        {
            IMongoCollection<T> collection = db.GetCollection<T>(GenerateCollectionName<T>());
            await collection.InsertOneAsync(entity);
            return entity;
        }

        public async Task<IEnumerable<T>> GetAllEntitiesAsync<T>() where T: IEntity, new()
        {
            IMongoCollection<T> collection = db.GetCollection<T>(GenerateCollectionName<T>());
            IAsyncCursor<T> task = await collection.FindAsync(e => true, null);
            return task.ToEnumerable();
        }

        public async Task<T> GetEntityWithIdAsync<T>(string id) where T: IEntity, new()
        {
            IMongoCollection<T> collection = db.GetCollection<T>(GenerateCollectionName<T>());
            IAsyncCursor<T> task = await collection.FindAsync(e => e.Id == id, null);
            return await task.FirstOrDefaultAsync();
        }

        public async Task<T> UpdateEntityAsync<T>(string id, T entity) where T : IEntity, new()
        {
            IMongoCollection<T> collection = db.GetCollection<T>(GenerateCollectionName<T>());
            var filter = Builders<T>.Filter.Eq("Id", id);
            await collection.ReplaceOneAsync(filter, entity);
            IAsyncCursor<T> task = await collection.FindAsync(filter);
            return await task.FirstAsync();
        }
    }
}