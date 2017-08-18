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
                 .UseActionTransform(actions => actions
                    .WithName("create-food-order")
                    .WithRepresentation("food-order")
                    .WithMethod(ActionMethod.Create)
                    .WithEncoding("application/x-www-form-urlencoded")
                    .WithLink<Restaurant, FoodOrdersController>(c => c.CreateFoodOrderAsync(null))
                    .WithField(x => x
                        .WithName(nameof(FoodOrderDto.RestaurantId))
                        .WithType("hidden")
                        .WithTitle("Restaurant Id")
                        .WithValue(r => r.Id)
                    )
               )
               .UseActionTransform(actions => actions
                    .WithName("edit")
                    .WithRepresentation("restaurant")
                    .WithMethod(ActionMethod.Replace)
                    .WithEncoding("application/x-www-form-urlencoded")
                    .WithLink<Restaurant, RestaurantsController>(r => c => c.UpdateAsync(r.Id, null))
                    .WithField(x => x
                        .WithName(nameof(RestaurantDto.Name))
                        .WithType("text")
                        .WithTitle("Name")
                    )
                    .WithField(x => x
                        .WithName(nameof(RestaurantDto.MenuLink))
                        .WithType("url")
                        .WithTitle("Menu Link")
                    )
               )
               .UseActionTransform(actions => actions
                    .WithName("delete")
                    .WithMethod(ActionMethod.Delete)
                    .WithLink<Restaurant, RestaurantsController>(r => c => c.DeleteAsync(r.Id))
               )
               .When((restaurant, request) => 
                    restaurant.CanHaveLinkToMenu(), 
                    whenConfig => whenConfig
                            .UseLinkTransform(links => links
                                .WithLink(l => l
                                .WithRelation("menu")
                                .WithTargetGenerator((r, h) => r.MenuLink)
                                )
                            )
               );
        }
    }
}