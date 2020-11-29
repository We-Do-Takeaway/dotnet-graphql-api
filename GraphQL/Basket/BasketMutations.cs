using System;
using System.Linq;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using WeDoTakeawayAPI.GraphQL.Common;
using WeDoTakeawayAPI.GraphQL.Model;
using WeDoTakeawayAPI.GraphQL.Extensions;

namespace WeDoTakeawayAPI.GraphQL.Basket
{
    [ExtendObjectType(Name = "Mutation")]
    // ReSharper disable once ClassNeverInstantiated.Global
    public class BasketMutations
    {
        [UseApplicationDbContext]
        public async Task<UpdateBasketPayload> AddBasketItemAsync(
            BasketItemInput input,
            [ScopedService] ApplicationDbContext dbContext)
        {
            var (basketId, itemId, quantity) = input;

            if (quantity == 0)
            {
                return new UpdateBasketPayload( new UserError("Invalid Item Quantity", "1003"));
            }

            Model.Basket basket = await dbContext
                .Baskets.Where(b => b.Id == basketId)
                .Include(b => b.BasketItems)
                .FirstOrDefaultAsync();

            if (basket == null)
            {
                return new UpdateBasketPayload( new UserError("Invalid Basket ID", "1001"));
            }

            // See if there is an item with this ID already, if so then increase it's quantity
            var item = basket.BasketItems.FirstOrDefault(i => i.ItemId == itemId);

            if (item != null)
            {
                item.Quantity += quantity;
            }
            else
            {
                item = new BasketItem
                {
                    BasketId = basketId,
                    ItemId = itemId,
                    Quantity = quantity
                };

                basket.BasketItems.Add(item);
            }

            await dbContext.SaveChangesAsync();

            return new UpdateBasketPayload(basket);
        }

        [UseApplicationDbContext]
        public async Task<UpdateBasketPayload> UpdateBasketItemAsync(
            BasketItemInput input,
            [ScopedService] ApplicationDbContext dbContext)
        {
            var (basketId, itemId, quantity) = input;

            Model.Basket basket = await dbContext
                .Baskets.Where(b => b.Id == basketId)
                .Include(b => b.BasketItems)
                .FirstOrDefaultAsync();

            if (basket == null)
            {
                return new UpdateBasketPayload( new UserError("Invalid Basket ID", "1001"));
            }

            // Check that the item exists
            var item = basket.BasketItems.FirstOrDefault(i => i.ItemId == itemId);

            if (item == null)
            {
                return new UpdateBasketPayload( new UserError("Invalid Basket Item ID", "1002"));
            }

            if (quantity > 0)
            {
                item.Quantity = quantity;
            }
            else
            {
                basket.BasketItems.Remove(item);
                dbContext.Remove(item);
            }

            await dbContext.SaveChangesAsync();

            return new UpdateBasketPayload(basket);
        }

        [UseApplicationDbContext]
        public async Task<UpdateBasketPayload> RemoveBasketItemAsync(
            BasketItemDeleteInput input,
            [ScopedService] ApplicationDbContext dbContext)
        {
            var (basketId, itemId) = input;

            Model.Basket basket = await dbContext
                .Baskets.Where(b => b.Id == basketId)
                .Include(b => b.BasketItems)
                .FirstOrDefaultAsync();

            if (basket == null)
            {
                return new UpdateBasketPayload( new UserError("Invalid Basket ID", "1001"));
            }

            // Check that the item exists
            var item = basket.BasketItems.FirstOrDefault(i => i.ItemId == itemId);

            if (item == null)
            {
                return new UpdateBasketPayload( new UserError("Invalid Basket Item ID", "1002"));
            }

            basket.BasketItems.Remove(item);
            dbContext.Remove(item);
            await dbContext.SaveChangesAsync();
            
            return new UpdateBasketPayload(basket);
        }

        [UseApplicationDbContext]
        public async Task<UpdateBasketPayload> ClearBasketAsync(
            Guid id,
            [ScopedService] ApplicationDbContext dbContext)
        {
            Model.Basket basket = await dbContext
                .Baskets.Where(b => b.Id == id)
                .Include(b => b.BasketItems)
                .FirstOrDefaultAsync();

            if (basket == null)
            {
                return new UpdateBasketPayload( new UserError("Invalid Basket ID", "1001"));
            }

            basket.BasketItems.Clear();

            await dbContext.SaveChangesAsync();
            return new UpdateBasketPayload(basket);
        }
    }
}
