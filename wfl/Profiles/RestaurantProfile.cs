using Hypermedia.Transforms;
using Hypermedia;
using Hypermedia.Affordances;
using Symphono.Wfl.Controllers;
using Symphono.Wfl.Models;

namespace Symphono.Wfl.Profiles
{
    public class RestaurantProfile: EntityProfile<Restaurant>
    {
        protected override void Configure(EntityConfiguration<Restaurant> configuration)
        {
            configuration
                .UseRepresentationTransform(representation => representation
                    .WithRepresentation("restaurant")
                )
                .UsePropertiesTransform(properties => properties
                    .WithProperty(r => r.Name)
                    .WithProperty(r => r.Id)
                )
                .UseLinkTransform(links => links
                    .WithLink(l => l
                        .WithRelation("self")
                        .WithRepresentation("restaurant")
                        .WithLink<Restaurant, RestaurantsController>(
                            r => rc => rc.GetByIdAsync(r.Id)
                        )
                    )
                 )
                .UseLinkTransform(links => links
                    .WithLink(l => l
                        .WithRelation("menu")
                        .WithTargetGenerator((r, h) => r.MenuLink)
                    )
               )
               .UseActionTransform(actions => actions
                    .WithName("create-food-order")
                    .WithRepresentation("food-order")
                    .WithMethod(ActionMethod.Create)
                    .WithEncoding("application/x-www-form-urlencoded")
                    .WithLink<Restaurant, FoodOrdersController>(c => c.CreateFoodOrderAsync(null))
                    .WithField(x => x
                        .WithName(nameof(FoodOrderDto.RestaurantId))
                        .WithType("text")
                        .WithTitle("Restaurant Id")
                        .WithValue(r => r.Id)
                    )
                );
        }
    }
}