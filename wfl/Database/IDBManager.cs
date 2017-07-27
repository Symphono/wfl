using System.Collections.Generic;
using System.Threading.Tasks;
using Symphono.Wfl.Models;
using System;

namespace Symphono.Wfl.Database
{
    public interface IDBManager<T> where T: IEntity
    {
        Task<T> InsertEntityAsync(T entity);
        Task<IEnumerable<T>> GetAllEntitiesAsync();
        Task<T> GetEntityByIdAsync(string id);
        Task<IEnumerable<T>> GetEntitiesByDateAsync(DateTime date);
        Task<T> UpdateEntityAsync(string id, T entity);
        DateTime GetCreationTime(string id);
    }
}
