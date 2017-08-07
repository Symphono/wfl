using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Symphono.Wfl.Models
{
    public class Restaurant: IEntity
    {
        [BsonIgnoreIfNull]
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Id { get; set; }
        public string Name { get; set; }
        public Uri MenuLink { get; set; }
        public bool CanHaveLinkToMenu()
        {
            if(MenuLink != null)
            {
                return true;
            }
            return false;
        }
    }
}