using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using WeDoTakeawayAPI.GraphQL.Extensions;
using WeDoTakeawayAPI.GraphQL.Model;

namespace WeDoTakeawayAPI.GraphQL.Order
{
    public class OrderType : ObjectType<Model.Order>
    {
        protected override void Configure(IObjectTypeDescriptor<Model.Order> descriptor)
        {
            descriptor
                .Field(o => o.OrderItems)
                .ResolveWith<OrderResolvers>(t =>
                    t.GetItemsAsync(
                        default!,
                        default!,
                        default!
                    )
                )
                .UseDbContext<ApplicationDbContext>()
                .Name("items");
        }

        private class OrderResolvers
        {

            public async Task<IEnumerable<OrderItem>> GetItemsAsync(
                Model.Order order,
                [ScopedService] ApplicationDbContext dbContext,
                CancellationToken cancellationToken)
            {
                // Get all the order item records for this order
                OrderItem[]? orderItems = await dbContext.OrderItems
                    .Where(oi => oi.OrderId == order.Id)
                    .Include(bi => bi.Item)
                    .ToArrayAsync(cancellationToken);

                return orderItems;
            }
        }
    }
}
