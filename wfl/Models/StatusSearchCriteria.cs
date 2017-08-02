using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace Symphono.Wfl.Models
{
    public class StatusSearchCriteria: ICriteria<FoodOrder>
    {
        public FoodOrder.StatusOptions Status { get; set;}

        public async Task<IEnumerable<FoodOrder>> ApplyCriteria(IMongoCollection<FoodOrder> collection)
        {
            IAsyncCursor<FoodOrder> task = await collection.FindAsync(o => o.Status == this.Status);
            IList<FoodOrder> orders = task.ToList();
            foreach (FoodOrder order in orders)
            {
                order.OnDeserialize();
            }
            return orders;
        }
    }
}