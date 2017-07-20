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
                    .WithProperty(s => s.Id)
                    .WithProperty(s => s.Description)
                    .WithProperty(s => s.OrdererName)
                    .WithProperty(s => s.FoodOrderId)
                )
                .UseLinkTransform(links => links
                    .WithLink(l => l
                        .WithRelation("self")
                        .WithRepresentation("menu-selection")
                        .WithLink<MenuSelection, MenuSelectionsController>(
                            s => mc => mc.GetByIdAsync(s.Id)
                        )
                    )
                 );
        }
    }
}