using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using WeDoTakeawayAPI.GraphQL.Extensions;
using WeDoTakeawayAPI.GraphQL.Model;
using WeDoTakeawayAPI.GraphQL.Section;
using WeDoTakeawayAPI.GraphQL.Section.DataLoaders;

namespace WeDoTakeawayAPI.GraphQL.Item
{
    public class ItemType : ObjectType<Model.Item>
    {
        protected override void Configure(IObjectTypeDescriptor<Model.Item> descriptor)
        {
            descriptor
                .Field(s => s.SectionItems)
                .ResolveWith <SectionResolvers> (t => 
                    t.GetSectionsAsync(
                        default!, 
                        default!, 
                        default!, 
                        default!
                    )
                )
                .UseDbContext<ApplicationDbContext>()
                .Name("sections");

        }
        
        private class SectionResolvers
        {
           public async Task<IEnumerable<Model.Section>> GetSectionsAsync(
                Model.Item item,
                [ScopedService] ApplicationDbContext dbContext,
                SectionByIdDataLoader sectionById,
                CancellationToken cancellationToken)
            {
                Guid[] sectionIds = await dbContext.Items
                    .Where(i => i.Id == item.Id)
                    .Include(i => i.SectionItems)
                    .SelectMany(i => i.SectionItems.Select(t => t.SectionId))
                    .ToArrayAsync(cancellationToken);
                
                return await sectionById.LoadAsync(sectionIds, cancellationToken);
            }
        }
    }
}