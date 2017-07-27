using Hypermedia.Transforms;
using Hypermedia;
using Hypermedia.Affordances;
using Symphono.Wfl.Controllers;
using Symphono.Wfl.Models;

namespace Symphono.Wfl.Profiles
{
    public class MenuSelectionProfile: EntityProfile<MenuSelection>
    {
        protected override void Configure(EntityConfiguration<MenuSelection> configuration)
        {
            configuration
                .UseRepresentationTransform(representation => representation
                    .WithRepresentation("menu-selection")
                )
                .UsePropertiesTransform(properties => properties
                    .WithProperty(s => s.Id)
                    .WithProperty(s => s.Description)
                    .WithProperty(s => s.OrdererName)
                )
                .UseSubEntityTransform(subentities => subentities 
                    .WithLinkedEntity(s => s.FoodOrder, config => config
                        .WithRelation("food-order")
                        .WithRepresentation("food-order")
                        .WithLink<MenuSelection, FoodOrder, FoodOrdersController>(s => fc => fc.GetByIdAsync(s.FoodOrder.Id))
                    )
                )
                .UseLinkTransform(links => links
                    .WithLink(l => l
                        .WithRelation("self")
                        .WithRepresentation("menu-selection")
                        .WithLink<MenuSelection, MenuSelectionsController>(
                            s => mc => mc.GetByIdAsync(s.FoodOrder.Id, s.Id)
                        )
                    )
                )
                .UseActionTransform(actions => actions
                    .WithName("delete-menu-selection")
                    .WithMethod(ActionMethod.Delete)
                    .WithEncoding("application/x-www-form-urlencoded")
                    .WithLink<MenuSelection, MenuSelectionsController>(s => mc => mc.DeleteByIdAsync(s.FoodOrder.Id, s.Index))
                );
        }
    }
}