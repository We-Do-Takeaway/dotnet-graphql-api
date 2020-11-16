using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using WeDoTakeawayAPI.GraphQL.Model;
using WeDoTakeawayAPI.GraphQL.Extensions;

namespace WeDoTakeawayAPI.GraphQL.Baskets
{
    [ExtendObjectType(Name = "Query")]
    public class BasketQueries
    {
        [UseApplicationDbContext]
        public async Task<Basket> GetBasketByCustomerId(
            Guid id,
            [ScopedService] ApplicationDbContext dbContext,
            CancellationToken cancellationToken)
        {
            // Look for a basket in the DB
            var customerId = id;
            
            Basket basket = await dbContext.Baskets
                .Where(b => b.CustomerId == customerId)
                .Include(b => b.Items)
                .FirstOrDefaultAsync(cancellationToken);

            // If we found one and it was correct, use it
            if (basket != null)
            {
                return basket;
            }

            // Otherwise create a new one for the customer in the db and return that
            basket = new Basket {Id = new Guid(), CustomerId = customerId};

            await dbContext.Baskets.AddAsync(basket, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return basket;
        }

    }
}
