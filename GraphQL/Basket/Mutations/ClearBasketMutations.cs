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
    public class ClearBasketMutations
    {
        [UseApplicationDbContext]
        public async Task<UpdateBasketPayload> ClearBasketByOwnerIdAsync(
            Guid id,
            [ScopedService] ApplicationDbContext dbContext)
        {
            Model.Basket basket = await dbContext
                .Baskets.Where(b => b.OwnerId == id)
                .Include(b => b.BasketItems)
                .FirstOrDefaultAsync();

            if (basket == null)
            {
                return new UpdateBasketPayload( new UserError("Invalid basket owner ID", "1001"));
            }

            basket.BasketItems.Clear();

            await dbContext.SaveChangesAsync();
            return new UpdateBasketPayload(basket);
        }
    }
}
