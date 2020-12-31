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
                var extensions = new Dictionary<string, object?>() {
                    { "code", "1001" },
                    {"id", id}

                };

                Error error = new("Invalid basket owner id", extensions: extensions);
                throw new QueryException(error);
            }

            basket.BasketItems.Clear();

            await dbContext.SaveChangesAsync();
            return new UpdateBasketPayload(basket);
        }
    }
}
