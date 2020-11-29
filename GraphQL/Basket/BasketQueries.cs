using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using WeDoTakeawayAPI.GraphQL.Model;
using WeDoTakeawayAPI.GraphQL.Extensions;

namespace WeDoTakeawayAPI.GraphQL.Basket
{
    [ExtendObjectType(Name = "Query")]
    public class BasketQueries
    {
        [UseApplicationDbContext]
        public async Task<Model.Basket> GetBasketByOwnerId(
            Guid id,
            [ScopedService] ApplicationDbContext dbContext,
            CancellationToken cancellationToken)
        {

            // Look for a basket in the DB
            Guid ownerId = id;

            Model.Basket basket = await dbContext.Baskets
                .Where(b => b.OwnerId == ownerId)
                .Include(b => b.BasketItems)
                .FirstOrDefaultAsync(cancellationToken);

            // If we found one and it was correct, use it
            if (basket != null)
            {
                return basket;
            }

            // Otherwise create a new one for the user in the db and return that
            // Todo Check for authenticated user and use that to say the basket is for an owner
            basket = new Model.Basket { Id = Guid.NewGuid(), OwnerId = ownerId, BasketType = BasketTypes.Anonymous };

            await dbContext.Baskets.AddAsync(basket, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return basket;
        }
    }
}
