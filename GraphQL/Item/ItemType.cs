using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using WeDoTakeawayAPI.GraphQL.Extensions;
using WeDoTakeawayAPI.GraphQL.Ingredient;
using WeDoTakeawayAPI.GraphQL.Model;
using WeDoTakeawayAPI.GraphQL.Section.DataLoaders;

namespace WeDoTakeawayAPI.GraphQL.Item
{
    public class ItemType : ObjectType<Model.Item> 
    {
        protected override void Configure(IObjectTypeDescriptor<Model.Item> descriptor)
        {
            descriptor
                .Field(i => i.SectionItems)
                .ResolveWith<ItemResolvers>(t => 
                    t.GetSectionsAsync(
                        default!,
                        default!,
                        default!,
                        default!
                        )
                    )
                .UseDbContext<ApplicationDbContext>()
                .Name("sections");

            descriptor
                .Field(i => i.ItemIngredients)
                .ResolveWith<ItemResolvers>(t =>
                    t.GetIngredientsAsync(
                        default!, 
                        default!, 
                        default!
                        )
                    )
                .UseDbContext<ApplicationDbContext>()
                .Name("ingredients");
            
        }

        private class ItemResolvers
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
            public async Task<IEnumerable<ItemIngredientWithQuantity>> GetIngredientsAsync(
                Model.Item item, 
                [ScopedService] ApplicationDbContext dbContext, 
                CancellationToken cancellationToken) 
            { 
                // Get all the itemingredient records and their associated ingredient records for this item
                var itemIngredients = await dbContext.ItemIngredients
                    .Where(ii => ii.ItemId == item.Id)
                    .Include(ii => ii.Ingredient)
                    .ToArrayAsync(cancellationToken);

                // Create a collection to store combined objects
                var itemIngredientsWithQuantity = new Collection<ItemIngredientWithQuantity>();
                
                // Loop through each result and combine the data so there is a single object with ingredient
                // and quantity
                foreach (var itemIngredient in itemIngredients) 
                { 
                    var expandedIngredient = new ItemIngredientWithQuantity(
                        itemIngredient.IngredientId, 
                        itemIngredient.Quantity ?? 0, 
                        itemIngredient.Ingredient!.Name ?? "", 
                        itemIngredient.Ingredient.Description, 
                        itemIngredient.Ingredient.Photo
                        );
                    
                    itemIngredientsWithQuantity.Add(expandedIngredient); 
                }

                return itemIngredientsWithQuantity.ToImmutableArray();
            }
        }
    }
}
