using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using WeDoTakeawayAPI.GraphQL.Extensions;
using WeDoTakeawayAPI.GraphQL.Item;
using WeDoTakeawayAPI.GraphQL.Item.DataLoaders;
using WeDoTakeawayAPI.GraphQL.Menu.DataLoaders;
using WeDoTakeawayAPI.GraphQL.Model;


namespace WeDoTakeawayAPI.GraphQL.Section
{
    public class SectionType : ObjectType<Model.Section>
    {
        protected override void Configure(IObjectTypeDescriptor<Model.Section> descriptor)
        {
            descriptor
                .Field(s => s.Menu)
                .ResolveWith<SectionResolvers>(t => 
                    t.GetMenuAsync(default!, default!, default!));
            
            descriptor
                .Field(s => s.SectionItems)
                .ResolveWith <SectionResolvers> (t => 
                    t.GetItemsAsync(
                        default!, 
                        default!, 
                        default!, 
                        default!
                    )
                )
                .UseDbContext<ApplicationDbContext>()
                .UsePaging<NonNullType<ItemType>>()
                .Name("items");
        }

        private class SectionResolvers
        {
            public async Task<Model.Menu?> GetMenuAsync(
                Model.Section section,
                MenuByIdDataLoader menuById,
                CancellationToken cancellationToken)
            {
                if (section.MenuId is null)
                {
                    return null;
                }

                return await menuById.LoadAsync(section.MenuId.Value, cancellationToken);
            }
            
            public async Task<IEnumerable<Model.Item>> GetItemsAsync(
                Model.Section section,
                [ScopedService] ApplicationDbContext dbContext,
                ItemByIdDataLoader itemById,
                CancellationToken cancellationToken)
            {

                var sectionWithSectionItems = await dbContext.Sections
                    .Where(s => s.Id == section.Id)
                    .Include(s => s.SectionItems)
                    .FirstOrDefaultAsync(cancellationToken);

                var itemIds = sectionWithSectionItems.SectionItems
                    .OrderBy(si => si.DisplayOrder)
                    .Select(si => si.ItemId)
                    .ToArray();
                
                return await itemById.LoadAsync(itemIds, cancellationToken);
            }
        }
    }
}