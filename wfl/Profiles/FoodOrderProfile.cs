using Hypermedia.Transforms;
using Hypermedia;
using Symphono.Wfl.Controllers;
using Symphono.Wfl.Models;

namespace Symphono.Wfl.Profiles
{
    public class FoodOrderProfile: EntityProfile<FoodOrder>
    {
        protected override void Configure(EntityConfiguration<FoodOrder> configuration)
        {
            configuration
                .UseRepresentationTransform(representation => representation
                    .WithRepresentation("food-order")
                )
                .UsePropertiesTransform(properties => properties
                    .WithProperty(o => o.Id)
                    .WithProperty(o => o.RestaurantId)
                )
                .UseLinkTransform(links => links
                    .WithLink(l => l
                        .WithRelation("self")
                        .WithRepresentation("food-order")
                        .WithLink<FoodOrder, FoodOrdersController>(
                            o => fc => fc.GetByIdAsync(o.Id)
                        )
                    )
                )
                .UseLinkTransform(links => links
                    .WithLink(l => l
                        .WithRelation("restaurant")
                        .WithRepresentation("restaurant")
                        .WithLink<FoodOrder, RestaurantsController>(
                            o => rc => rc.GetByIdAsync(o.RestaurantId)
                        )
                    )
                );
        }
    }
}