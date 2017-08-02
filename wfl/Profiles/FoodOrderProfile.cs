using Hypermedia.Transforms;
using Hypermedia.Affordances;
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
                .UseEnumerableEmbeddedSubEntityTransform(
                    o => o.MenuSelections,
                    embedded => embedded
                         .WithRelation("item")
                         .WithConfiguration(embeddedConfiguration => embeddedConfiguration
                             .UseProfile<MenuSelection, MenuSelectionProfile>()
                        )
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
                 .UseActionTransform(actions => actions
                    .WithName("create-menu-selection")
                    .WithRepresentation("menu-selection")
                    .WithMethod(ActionMethod.Create)
                    .WithEncoding("application/x-www-form-urlencoded")
                    .WithLink<FoodOrder, MenuSelectionsController>(o => mc => mc.CreateMenuSelectionAsync(null, o.Id))
                    .WithField(x => x
                        .WithName(nameof(MenuSelectionDto.OrdererName))
                        .WithType("text")
                        .WithTitle("Orderer Name")
                    )
                    .WithField(x => x
                        .WithName(nameof(MenuSelectionDto.Description))
                        .WithType("text")
                        .WithTitle("Description")
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