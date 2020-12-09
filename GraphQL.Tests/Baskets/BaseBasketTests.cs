using System;
using System.Threading.Tasks;
using WeDoTakeawayAPI.GraphQL.Model;


namespace WeDoTakeawayAPI.GraphQL.Tests.Baskets
{
    public class BaseBasketTests : BaseTests
    {
        protected async Task CreateBasketWithItem(Guid basketId, Guid ownerId, Guid itemId)
        {
            ApplicationDbContext dbContext = GetDbContext();

            var basket = new Model.Basket { Id = basketId, OwnerId = ownerId, BasketType = BasketTypes.Anonymous };
            await dbContext.Baskets.AddAsync(basket);
            await dbContext.SaveChangesAsync();

            var item = new BasketItem { BasketId = basketId, ItemId = itemId, Quantity = 1 };
            await dbContext.BasketItems.AddAsync(item);
            await dbContext.SaveChangesAsync();
        }

        protected async Task CreateEmptyBasket(Guid basketId, Guid ownerId)
        {
            ApplicationDbContext dbContext = GetDbContext();
            var basket = new Model.Basket { Id = basketId, OwnerId = ownerId, BasketType = BasketTypes.Anonymous };
            await dbContext.Baskets.AddAsync(basket);
            await dbContext.SaveChangesAsync();
        }

        protected async Task AddItem(Guid basketId, Guid itemId, int quantity = 1)
        {
            var item = new BasketItem
            {
                BasketId = basketId,
                ItemId = itemId,
                Quantity = quantity
            };

            ApplicationDbContext dbContext = GetDbContext();

            Model.Basket basket = await dbContext.Baskets.FindAsync(basketId);
            basket.BasketItems.Add(item);
            await dbContext.SaveChangesAsync();
        }
    }
}
