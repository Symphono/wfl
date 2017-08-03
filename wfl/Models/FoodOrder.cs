﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System.Collections.Generic;
using MongoDB.Bson;

namespace Symphono.Wfl.Models
{
    public class FoodOrder: IContainerEntity
    {
        [BsonIgnoreIfNull]
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Id { get; set; }
        public string RestaurantId { get; set; }
        public IList<MenuSelection> MenuSelections { get; set; }
        public void OnDeserialize()
        {
            if (MenuSelections != null)
            {
                foreach (MenuSelection selection in MenuSelections)
                {
                    selection.FoodOrder = this;
                }
            }
        }
        public void addMenuSelection(MenuSelection selection)
        {
            selection.Id = ObjectId.GenerateNewId().ToString();
            if (MenuSelections == null)
            {
                MenuSelections = (new[] { selection });
            }
            else
            {
                MenuSelections.Add(selection);
            }
        }
    }
}