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

namespace WeDoTakeawayAPI.GraphQL.Order
{
    [ExtendObjectType(Name = "Mutation")]
    public class AddOrderMutation
    {
        [UseApplicationDbContext]
        public async Task<AddOrderPayload> AddOrderAsync(AddOrderInput input,
            [ScopedService] ApplicationDbContext dbContext)
        {
            // Check that there are quantities
            if (!input.Items.Any())
            {
                Error error = new("No items to order", "1010");
                throw new QueryException(error);
            }

            // Check the quantities are valid
            if (input.Items.Any(item => item.Quantity < 1 || item.Quantity > 99))
            {
                Error error = new ("Invalid order item quantity", "1011");
                throw new QueryException(error);
            }

            // Check that the items provided exist in the db
            var itemIds = input.Items.Select(item => item.ItemId).ToList();
            Model.Item[]? dbItems = await dbContext.Items
                .Where(i => itemIds.Contains(i.Id)).ToArrayAsync();

            if (dbItems.Length != input.Items.Count())
            {
                Error error = new("Invalid order item id", "1012");
                throw new QueryException(error);
            }

            // Create the order object.
            var orderId = Guid.NewGuid();
            Model.Order order = new Model.Order
            {
                Id = orderId,
                Name = input.Name,
                Address1 = input.Address1,
                Address2 = input.Address2,
                Town = input.Town,
                Postcode = input.Postcode,
                Phone = input.Phone,
                Email = input.Email,
                DeliveryInstructions = input.DeliveryInstructions,
                OwnerId = input.OwnerId,
                CreatedAt = DateTime.Now
            };

            await dbContext.Orders.AddAsync(order);


            IEnumerable<OrderItemInput> items = input.Items;

            // Now add the order items
            foreach ((Guid itemId, var quantity) in items)
            {
                // Get a snapshot of the item when ordered
                Model.Item dbItem = dbItems.First(item => item.Id == itemId);

                OrderItem orderItem = new OrderItem
                {
                    OrderId = orderId,
                    ItemId = itemId,
                    Quantity = quantity,
                    Name = dbItem?.Name,
                    Description = dbItem?.Description
                };

                await dbContext.OrderItems.AddAsync(orderItem);
            }

            await dbContext.SaveChangesAsync();
            return new AddOrderPayload(order);
        }
    }
}
