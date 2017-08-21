using MongoDB.Driver;

namespace Symphono.Wfl.Models
{
    public class NameSearchCriteria : ICriteria<Restaurant>
    {
        public string Name { get; set; }
        public FilterDefinition<Restaurant> CreateFilter()
        {
            return Builders<Restaurant>.Filter.Eq(nameof(Restaurant.Name), this.Name);
        }

        public bool HasCriteria()
        {
            if(string.IsNullOrEmpty(Name))
            {
                return false;
            }
            return true;
        }
    }
}