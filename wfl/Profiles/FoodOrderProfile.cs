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
                    .WithProperty(o => o.Status)
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
                .UseLinkTransform(links => links
                    .WithLink(l => l
                        .WithRelation("restaurant")
                        .WithRepresentation("restaurant")
                        .WithLink<FoodOrder, RestaurantsController>(
                            o => rc => rc.GetByIdAsync(o.RestaurantId)
                        )
                    )
                )
                .When(
                    (o, request) => o.Status == FoodOrder.StatusOptions.Active,
                    config => config
                        .UseActionTransform(actions => actions
                            .WithName("discard")
                            .WithMethod(ActionMethod.Create)
                            .WithLink<FoodOrder, FoodOrdersController>(o => fc => fc.SetStatusAsync(o.Id, null))
                            .WithField(x => x
                                .WithName(nameof(FoodOrderStatusDto.Status))
                                .WithType("hidden")
                                .WithValue(FoodOrder.StatusOptions.Discarded)
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
                )
                .When(
                    (o, request) => o.Status == FoodOrder.StatusOptions.Discarded,
                    config => config
                        .UseActionTransform(actions => actions
                            .WithName("reactivate")
                            .WithMethod(ActionMethod.Create)
                            .WithLink<FoodOrder, FoodOrdersController>(o => fc => fc.SetStatusAsync(o.Id, null))
                            .WithField(x => x
                                .WithName(nameof(FoodOrderStatusDto.Status))
                                .WithType("hidden")
                                .WithValue(FoodOrder.StatusOptions.Active)
                            )
                        )
                );
        }
    }
}