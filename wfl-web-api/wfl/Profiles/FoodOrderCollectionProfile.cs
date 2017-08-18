using Hypermedia.Affordances;
using Hypermedia.Transforms;
using Hypermedia;
using Symphono.Wfl.Models;
using Symphono.Wfl.Controllers;
using System;

namespace Symphono.Wfl.Profiles
{
    public class FoodOrderCollectionProfile : EntityProfile<FoodOrderCollection>
    {
        protected override void Configure(EntityConfiguration<FoodOrderCollection> configuration)
        {
            configuration
                .UseRepresentationTransform(representation => representation
                    .WithRepresentation("food-order-collection")
                    .WithRepresentation("collection")
                )
                .UseEnumerableEmbeddedSubEntityTransform(
                    c => c.FoodOrders,
                    embedded => embedded
                        .WithRelation("item")
                        .WithConfiguration(embeddedConfiguration => embeddedConfiguration
                            .UseProfile<FoodOrder, FoodOrderProfile>()
                        )
                )
                .WithLink<FoodOrderCollection, FoodOrdersController>(
                    "self",
                    c => fc => fc.GetAsync(c.Criteria)
                )
                .WithLink<FoodOrderCollection, FoodOrdersController>(
                    "status-options", 
                    o => fc => fc.GetStatusOptions()
                )
                .UseActionTransform(actions => actions
                    .WithName("filter-food-orders")
                    .WithRepresentation("collection")
                    .WithMethod(ActionMethod.Read)
                    .WithEncoding("application/x-www-form-urlencoded")
                    .WithLink<FoodOrderCollection, FoodOrdersController>(c => c.GetAsync(null))
                    .WithField(x => x
                        .WithName(nameof(FoodOrder.Status))
                        .WithType("text")
                        .WithTitle("Status")
                    )
                );
        }
    }
}