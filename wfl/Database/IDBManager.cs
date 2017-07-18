using System.Collections.Generic;
using System.Threading.Tasks;
using Symphono.Wfl.Models;
using System;

namespace Symphono.Wfl.Database
{
    public interface IDBManager
    {
        Task<T> InsertEntityAsync<T>(T entity) where T : IEntity;
        Task<IEnumerable<T>> GetAllEntitiesAsync<T>() where T : IEntity;
        Task<T> GetEntityByIdAsync<T>(string id) where T : IEntity;
        Task<T> DeleteEntityByIdAsync<T>(string id) where T : IEntity;
        Task<IEnumerable<T>> GetEntitiesByDateAsync<T>(DateTime date) where T : IEntity;
        Task<T> UpdateEntityAsync<T>(string id, T entity) where T: IEntity;
    }
}
