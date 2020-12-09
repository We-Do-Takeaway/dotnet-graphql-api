using System;
using System.Linq;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using WeDoTakeawayAPI.GraphQL.Common;
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
                return new UpdateBasketPayload( new UserError("Invalid basket owner ID", "1001"));
            }

            // Check that the item exists
            BasketItem? item = basket.BasketItems.FirstOrDefault(i => i.ItemId == itemId);

            if (item == null)
            {
                return new UpdateBasketPayload( new UserError("Invalid Basket Item ID", "1002"));
            }

            basket.BasketItems.Remove(item);
            dbContext.Remove(item);
            await dbContext.SaveChangesAsync();

            return new UpdateBasketPayload(basket);
        }
    }
}
