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

namespace WeDoTakeawayAPI.GraphQL.Menu
{
    public class MenuType : ObjectType<Model.Menu>
    {
        protected override void Configure(IObjectTypeDescriptor<Model.Menu> descriptor)
        {
            descriptor
                .Field(t => t.Sections)
                .ResolveWith <MenuResolvers> (t => 
                    t.GetSectionsAsync(
                        default!, 
                        default!, 
                        default!, 
                        default!
                        )
                    )
                .UseDbContext<ApplicationDbContext>()
                .UsePaging<NonNullType<SectionType>>()
                .Name("sections");
        }

        private class MenuResolvers
        {
            public async Task<IEnumerable<Model.Section>> GetSectionsAsync(
                Model.Menu menu,
                [ScopedService] ApplicationDbContext dbContext,
                SectionByIdDataLoader sectionById,
                CancellationToken cancellationToken)
            {
                Guid[] sectionIds = await dbContext.Sections
                    .Where(s => s.MenuId == menu.Id)
                    .OrderBy(s => s.DisplayOrder)
                    .Select(s => s.Id)
                    .ToArrayAsync(cancellationToken);
                
                return await sectionById.LoadAsync(sectionIds, cancellationToken);
            }
    }
    }
}