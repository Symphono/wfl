using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Symphono.Wfl.Models
{
    public interface ICriteria<E> where E: IEntity
    {
        Task<IEnumerable<E>> ApplyCriteria(IMongoCollection<E> collection);
    }
}
