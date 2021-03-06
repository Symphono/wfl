﻿using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using Symphono.Wfl.Models;
using System;
using System.Linq;

namespace Symphono.Wfl.Database
{
    public class MongoDBManager<T>: IDBManager<T> where T: IEntity
    {
        private static string connectionString;
        public static MongoClient client;
        public static IMongoDatabase db;
        private readonly string collectionName;
        
        public MongoDBManager(string inputCollectionName, string inputConnectionString)
        {
            connectionString = inputConnectionString;
            client = new MongoClient(connectionString);
            db = client.GetDatabase("WFL");
            collectionName = inputCollectionName;
        }

        public async Task<T> InsertEntityAsync(T entity)
        {
            IMongoCollection<T> collection = db.GetCollection<T>(collectionName);
            await collection.InsertOneAsync(entity);
            return entity;
        }

        public async Task<IEnumerable<T>> GetEntitiesAsync(ICriteria<T> criteria)
        {
            IMongoCollection<T> collection = db.GetCollection<T>(collectionName);
            IAsyncCursor<T> task;
            if (criteria != null)
            {
               task = await collection.FindAsync(criteria.CreateFilter(), null);
            }
            else
            {
                task = await collection.FindAsync(e => true, null);
            }
            IList<T> entities = task.ToList();
            entities.OfType<IContainerEntity>().ToList().ForEach(entity => entity.OnDeserialize());
            return entities;
        }

        public async Task<T> GetEntityByIdAsync(string id)
        {
            IMongoCollection<T> collection = db.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Eq(nameof(IEntity.Id), id);
            IAsyncCursor<T> task = await collection.FindAsync(filter, null);
            T entity = await task.FirstOrDefaultAsync();
            if (entity is IContainerEntity)
            {
                (entity as IContainerEntity).OnDeserialize();
            }
            return entity;
        }

        public async Task<IEnumerable<T>> GetEntitiesByDateAsync(DateTime date)
        {
            IMongoCollection<T> collection = db.GetCollection<T>(collectionName);
            var lowerBound = Builders<T>.Filter.Gt(nameof(IEntity.Id), new ObjectId(date.Date, 0, 0, 0));
            var upperBound = Builders<T>.Filter.Lt(nameof(IEntity.Id), new ObjectId(date.Date.AddDays(1), 0, 0, 0));
            var bounds = new FilterDefinition<T>[] { lowerBound, upperBound };
            var filter = Builders<T>.Filter.And(bounds);
            
            IAsyncCursor<T> task = await collection.FindAsync(lowerBound, null);
            return task.ToEnumerable();
        }

        public async Task<T> UpdateEntityAsync(string id, T entity)
        {
            IMongoCollection<T> collection = db.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Eq(nameof(IEntity.Id), id);
            await collection.ReplaceOneAsync(filter, entity);
            return entity;
        }

        public async Task<T> DeleteEntityByIdAsync(string id)
        {
            IMongoCollection<T> collection = db.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Eq(nameof(IEntity.Id), id);
            return await collection.FindOneAndDeleteAsync(filter);
        }

        public DateTime GetCreationTime(string id)
        {
            return new ObjectId(id).CreationTime;
        }
    }
}