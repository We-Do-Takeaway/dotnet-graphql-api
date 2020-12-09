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
                return new UpdateBasketPayload( new UserError("Invalid Item Quantity", "1003"));
            }

            Model.Basket basket = await dbContext
                .Baskets.Where(b => b.OwnerId == ownerId)
                .Include(b => b.BasketItems)
                .FirstOrDefaultAsync();

            if (basket == null)
            {
                return new UpdateBasketPayload( new UserError("Invalid basket owner ID", "1001"));
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
