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
                .UseLinkTransform(links => links
                    .WithLink(l => l
                        .WithRelation("self")
                        .WithRepresentation("food-order")
                        .WithLink<FoodOrder, FoodOrdersController>(
                            o => rc => rc.GetByIdAsync(o.Id)
                        )
                    )
                 )
                 .UseActionTransform(actions => actions
                    .WithName("create-menu-selection")
                    .WithRepresentation("menu-selection")
                    .WithMethod(ActionMethod.Create)
                    .WithEncoding("application/x-www-form-urlencoded")
                    .WithLink<FoodOrder, MenuSelectionsController>(o => mc => mc.CreateMenuSelectionAsync(null))
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
                    .WithField(x => x
                        .WithName(nameof(MenuSelectionDto.FoodOrderId))
                        .WithType("text")
                        .WithTitle("Food Order Id")
                        .WithValue(o => o.Id)
                    )
               );
        }
    }
}