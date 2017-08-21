using Hypermedia.Affordances;
using Hypermedia.Transforms;
using Hypermedia;
using Symphono.Wfl.Controllers;
using Symphono.Wfl.Models;

namespace Symphono.Wfl.Profiles
{
    public class RootProfile: EntityProfile<Root>
    {
        protected override void Configure(EntityConfiguration<Root> configuration)
        {
            configuration
                .UseRepresentationTransform(representation => representation
                    .WithRepresentation("root")
                    .WithRepresentation("collection")
                )
                .UseLinkTransform(links => links
                    .WithLink(l => l
                        .WithRelation("self")
                        .WithRepresentation("root")
                        .WithLink<Root, RootController>(
                            root => rc => rc.Get()
                        )
                    )
               )
               .UseLinkTransform(links => links
                    .WithLink(l => l
                        .WithRelation("restaurants")
                        .WithRepresentation("restaurant")
                        .WithLink<Root, RestaurantsController>(
                            root => rc => rc.GetAsync(null)
                        )
                    )
               )
               .UseLinkTransform(links => links
                    .WithLink(l => l
                        .WithRelation("food-orders")
                        .WithRepresentation("food-order")
                        .WithLink<Root, FoodOrdersController>(
                            root => fc => fc.GetAsync(null)
                        )
                    )
               );
        }
    }
}