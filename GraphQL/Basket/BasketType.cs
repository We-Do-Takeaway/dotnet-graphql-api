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
using WeDoTakeawayAPI.GraphQL.Model;

namespace WeDoTakeawayAPI.GraphQL.Basket
{
    public class BasketType : ObjectType<Model.Basket>
    {
        protected override void Configure(IObjectTypeDescriptor<Model.Basket> descriptor)
        {
            descriptor
                .Field(b => b.BasketItems)
                .Type<NonNullType<ListType<NonNullType<BasketItemExpandedType>>>>()
                .ResolveWith<BasketResolvers>(t =>
                    t.GetItemsAsync(
                        default!,
                        default!,
                        default!
                    )
                )
                .UseDbContext<ApplicationDbContext>()
                .Name("items")
                ;
        }

        private class BasketResolvers
        {

            public async Task<IEnumerable<BasketItemExpanded>> GetItemsAsync(
                Model.Basket basket,
                [ScopedService] ApplicationDbContext dbContext,
                CancellationToken cancellationToken)
            {
                // Get all the basketitem records and their associated item records for this basket
                BasketItem[] basketItems = await dbContext.BasketItems
                    .Where(bi => bi.BasketId == basket.Id)
                    .Include(bi => bi.Item)
                    .ToArrayAsync(cancellationToken);

                // Create a collection to store combined objects
                var basketItemsExpanded = new Collection<BasketItemExpanded>();

                // Loop through each result and combine the data so thee is a basket item with all the item fields and the quantity
                foreach (var basketItem in basketItems)
                {
                    var expandedBasketItem = new BasketItemExpanded(
                        basketItem.ItemId,
                        basketItem.Quantity,
                        basketItem.Item?.Name ?? "",
                        basketItem.Item?.Description ,
                        basketItem.Item?.Photo
                        );

                    basketItemsExpanded.Add(expandedBasketItem);
                }

                return basketItemsExpanded.ToImmutableArray();
            }
        }
    }
}
