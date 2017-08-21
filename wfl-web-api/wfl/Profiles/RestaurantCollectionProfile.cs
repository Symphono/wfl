using Hypermedia.Transforms;
using Hypermedia;
using Hypermedia.Affordances;
using Symphono.Wfl.Controllers;
using Symphono.Wfl.Models;
using System;

namespace Symphono.Wfl.Profiles
{
    public class RestaurantCollectionProfile : EntityProfile<RestaurantCollection>
    {
        protected override void Configure(EntityConfiguration<RestaurantCollection> configuration)
        {
            configuration
                .UseRepresentationTransform(representation => representation
                    .WithRepresentation("restaurant-collection")
                    .WithRepresentation("collection")
                )
                .UseEnumerableEmbeddedSubEntityTransform(
                    rc => rc.Restaurants,
                    embedded => embedded
                        .WithRelation("item")
                        .WithConfiguration(embeddedConfiguration => embeddedConfiguration
                            .UseProfile<Restaurant, RestaurantProfile>()
                        )
                )
                .WithLink<RestaurantCollection, RestaurantsController>(
                    "self",
                    rc => c => c.GetAsync(rc.Criteria)
                )
                .UseActionTransform(actions => actions
                    .WithName("create-restaurant")
                    .WithRepresentation("restaurant")
                    .WithMethod(ActionMethod.Create)
                    .WithEncoding("application/x-www-form-urlencoded")
                    .WithLink<RestaurantCollection, RestaurantsController>(c => c.CreateRestaurantAsync(null))
                    .WithField(x => x
                        .WithName(nameof(RestaurantDto.Name))
                        .WithType("text")
                        .WithTitle("Name")
                    )
                    .WithField(x => x
                        .WithName(nameof(RestaurantDto.MenuLink))
                        .WithType("text")
                        .WithTitle("Menu Link")
                    )
                )
                .UseActionTransform(actions => actions
                    .WithName("filter-restaurants")
                    .WithRepresentation("collection")
                    .WithMethod(ActionMethod.Read)
                    .WithEncoding("application/x-www-form-urlencoded")
                    .WithLink<RestaurantCollection, RestaurantsController>(c => c.GetAsync(null))
                    .WithField(x => x
                        .WithName(nameof(Restaurant.Name))
                        .WithType("text")
                        .WithTitle("Name")
                    )
                );
        }
    }
}