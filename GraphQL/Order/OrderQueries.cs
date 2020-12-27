using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using WeDoTakeawayAPI.GraphQL.Model;
using WeDoTakeawayAPI.GraphQL.Extensions;

namespace WeDoTakeawayAPI.GraphQL.Order
{
    [ExtendObjectType(Name = "Query")]
    public class OrderQueries
    {
        [UseApplicationDbContext]
        public async Task<Model.Order> GetOrderById(
            Guid id,
            [ScopedService] ApplicationDbContext dbContext,
            CancellationToken cancellationToken)
        {

            Model.Order order = await dbContext.Orders
                .Where(o => o.Id == id)
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(cancellationToken);

            return order;
        }
    }
}
