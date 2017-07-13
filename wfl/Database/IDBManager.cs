using System.Collections.Generic;
using System.Threading.Tasks;
using Symphono.Wfl.Models;
using System;

namespace Symphono.Wfl.Database
{
    public interface IDBManager
    {
        Task<T> InsertEntityAsync<T>(T entity) where T : IEntity, new();
        Task<IEnumerable<T>> GetAllEntitiesAsync<T>() where T : IEntity, new();
        Task<T> GetEntityWithIdAsync<T>(string id) where T : IEntity, new();
        Task<T> UpdateEntityAsync<T>(string id, T entity) where T: IEntity, new();
    }
}
