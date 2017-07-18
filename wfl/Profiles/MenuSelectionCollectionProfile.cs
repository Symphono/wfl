using Hypermedia.Transforms;
using Hypermedia;
using Hypermedia.Affordances;
using Symphono.Wfl.Controllers;
using Symphono.Wfl.Models;
using System;

namespace Symphono.Wfl.Profiles
{
    public class MenuSelectionCollectionProfile: EntityProfile<MenuSelectionCollection>
    {
        protected override void Configure(EntityConfiguration<MenuSelectionCollection> configuration)
        {
            configuration
               .UseRepresentationTransform(representation => representation
                    .WithRepresentation("menu-selection-collection")
                    .WithRepresentation("collection")
               )
               .UseEnumerableEmbeddedSubEntityTransform(
                    mc => mc.MenuSelections,
                    embedded => embedded
                         .WithRelation("item")
                         .WithConfiguration(embeddedConfiguration => embeddedConfiguration
                             .UseProfile<MenuSelection, MenuSelectionProfile>()
                        )
               )
               .WithLink<MenuSelectionCollection, MenuSelectionsController>(
                    "self",
                     s => c => c.GetAsync()
               )
               .UseActionTransform(actions => actions
                    .WithName("create-menu-selection")
                    .WithRepresentation("menu-selection")
                    .WithMethod(ActionMethod.Create)
                    .WithEncoding("application/x-www-form-urlencoded")
                    .WithLink<MenuSelectionCollection, MenuSelectionsController>(c => mc => mc.CreateMenuSelectionAsync(null))
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
                    )
               );
        }
    }
}