using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using WeDoTakeawayAPI.GraphQL.Extensions;
using WeDoTakeawayAPI.GraphQL.Model;

namespace WeDoTakeawayAPI.GraphQL.Basket.Mutations
{
    [ExtendObjectType(Name = "Mutation")]
    public class AddBasketItemMutations
    {
        [UseApplicationDbContext]
        public async Task<UpdateBasketPayload> AddBasketItemAsync(
            BasketItemInput input,
            [ScopedService] ApplicationDbContext dbContext)
        {
            (Guid ownerId, Guid itemId, var quantity) = input;

            if (quantity == 0)
            {
                Error error = new("Invalid item quantity", "1003");
                throw new QueryException(error);
            }

            Model.Basket basket = await dbContext
                .Baskets.Where(b => b.OwnerId == ownerId)
                .Include(b => b.BasketItems)
                .FirstOrDefaultAsync();

            if (basket == null)
            {
                var extensions = new Dictionary<string, object?>() {
                    { "code", "1001" },
                    {"id", ownerId}
                };

                Error error = new("Invalid basket owner id", extensions: extensions);
                throw new QueryException(error);
            }

            // See if there is an item with this ID already, if so then increase it's quantity
            BasketItem? item = basket.BasketItems.FirstOrDefault(i => i.ItemId == itemId);

            if (item != null)
            {
                item.Quantity += quantity;
            }
            else
            {
                item = new BasketItem
                {
                    BasketId = basket.Id,
                    ItemId = itemId,
                    Quantity = quantity
                };

                basket.BasketItems.Add(item);
            }

            await dbContext.SaveChangesAsync();

            return new UpdateBasketPayload(basket);
        }
    }
}
