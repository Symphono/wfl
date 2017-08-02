using Hypermedia.Transforms;
using Hypermedia;
using Symphono.Wfl.Models;
using Symphono.Wfl.Controllers;
using System;

namespace Symphono.Wfl.Profiles
{
    public class StatusOptionsRepresentationProfile: EntityProfile<StatusOptionsRepresentation>
    {
        protected override void Configure(EntityConfiguration<StatusOptionsRepresentation> configuration)
        {
            configuration
                .UseRepresentationTransform(representation => representation
                    .WithRepresentation("status-options")
                )
                .UsePropertiesTransform(properties => properties
                    .WithProperty(o => o.values)
                )
                .UseLinkTransform(links => links
                    .WithLink(l => l
                        .WithRelation("self")
                        .WithRepresentation("status-options")
                        .WithLink<StatusOptionsRepresentation, FoodOrdersController>(o => c => c.GetStatusOptions())
                    )
                );
        }
    }
}