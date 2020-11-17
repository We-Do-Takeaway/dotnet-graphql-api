using System;
using System.Linq;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using WeDoTakeawayAPI.GraphQL.Common;
using WeDoTakeawayAPI.GraphQL.Model;
using WeDoTakeawayAPI.GraphQL.Extensions;

namespace WeDoTakeawayAPI.GraphQL.Baskets
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
            var (basketId, itemId, qty) = input;
            
            if (qty == 0)
            {
                return new UpdateBasketPayload( new UserError("Invalid Item Quantity", "1003"));
            }
            
            Basket basket = await dbContext
                .Baskets.Where(b => b.Id == basketId)
                .Include(b => b.Items)
                .FirstOrDefaultAsync();

            if (basket == null)
            {
                return new UpdateBasketPayload( new UserError("Invalid Basket ID", "1001"));
            }
            
            // See if there is an item with this ID already, if so then increase it's qty
            var item = basket.Items.FirstOrDefault(i => i.ItemId == itemId);

            if (item != null)
            {
                item.Qty += qty;
            }
            else
            {
                item = new BasketItem
                {
                    BasketId = basketId,
                    ItemId = itemId,
                    Qty = qty
                };
                
                basket.Items.Add(item);
            }
            
            await dbContext.SaveChangesAsync();
            
            return new UpdateBasketPayload(basket);
        }
        
        [UseApplicationDbContext]
        public async Task<UpdateBasketPayload> UpdateBasketItemAsync(
            BasketItemInput input,
            [ScopedService] ApplicationDbContext dbContext)
        {
            var (basketId, itemId, qty) = input;
            
            Basket basket = await dbContext
                .Baskets.Where(b => b.Id == basketId)
                .Include(b => b.Items)
                .FirstOrDefaultAsync();

            if (basket == null)
            {
                return new UpdateBasketPayload( new UserError("Invalid Basket ID", "1001"));
            }
            
            // Check that the item exists
            var item = basket.Items.FirstOrDefault(i => i.ItemId == itemId);

            if (item == null)
            {
                return new UpdateBasketPayload( new UserError("Invalid Basket Item ID", "1002"));
            }

            if (qty > 0)
            {
                item.Qty = qty;
            }
            else
            {
                basket.Items.Remove(item);
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
            
            Basket basket = await dbContext
                .Baskets.Where(b => b.Id == basketId)
                .Include(b => b.Items)
                .FirstOrDefaultAsync();

            if (basket == null)
            {
                return new UpdateBasketPayload( new UserError("Invalid Basket ID", "1001"));
            }
            
            // Check that the item exists
            var item = basket.Items.FirstOrDefault(i => i.ItemId == itemId);

            if (item == null)
            {
                return new UpdateBasketPayload( new UserError("Invalid Basket Item ID", "1002"));
            }

            basket.Items.Remove(item);
            dbContext.Remove(item);
            
            return new UpdateBasketPayload(basket);
        }
        
        [UseApplicationDbContext]
        public async Task<UpdateBasketPayload> ClearBasketAsync(
            Guid id,
            [ScopedService] ApplicationDbContext dbContext)
        {
            Basket basket = await dbContext
                .Baskets.Where(b => b.Id == id)
                .Include(b => b.Items)
                .FirstOrDefaultAsync();

            if (basket == null)
            {
                return new UpdateBasketPayload( new UserError("Invalid Basket ID", "1001"));
            }
            
            basket.Items.Clear();

            await dbContext.SaveChangesAsync();
            return new UpdateBasketPayload(basket);
        }
    }
}
