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
    public class RemoveBasketItemMutations
    {
        [UseApplicationDbContext]
        public async Task<UpdateBasketPayload> RemoveBasketItemAsync(
            BasketItemDeleteInput input,
            [ScopedService] ApplicationDbContext dbContext)
        {
            (Guid ownerId, Guid itemId) = input;

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

            // Check that the item exists
            BasketItem? item = basket.BasketItems.FirstOrDefault(i => i.ItemId == itemId);

            if (item == null)
            {
                var extensions = new Dictionary<string, object?>() {
                    { "code", "1002" },
                    {"id", itemId}
                };

                Error error = new("Invalid basket item id", extensions: extensions);
                throw new QueryException(error);
   }

            basket.BasketItems.Remove(item);
            dbContext.Remove(item);
            await dbContext.SaveChangesAsync();

            return new UpdateBasketPayload(basket);
        }
    }
}
