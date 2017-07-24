using Hypermedia.Transforms;
using Hypermedia;
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
                    .WithProperty(s => s.Index)
                    .WithProperty(s => s.Description)
                    .WithProperty(s => s.OrdererName)
                    .WithProperty(s => s.FoodOrder)
                )
                /*.UseSubEntityTransform(subentities => subentities
                    .WithEmbeddedEntity(
                        s => s.FoodOrder, 
                        subentityconfig => subentityconfig
                            .WithRelation("food-order")
                            .WithRepresentation("food-order")
                            .WithConfiguration(embeddedConfiguration => embeddedConfiguration
                                .UseProfile<FoodOrder, FoodOrderProfile>()
                            )
                    )
                )*/
                .UseLinkTransform(links => links
                    .WithLink(l => l
                        .WithRelation("self")
                        .WithRepresentation("menu-selection")
                        .WithLink<MenuSelection, MenuSelectionsController>(
                            s => mc => mc.GetByIndexAsync(s.FoodOrder.Id, s.Index)
                        )
                    )
                 );
        }
    }
}