using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Execution;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using WeDoTakeawayAPI.GraphQL.Model;
using WeDoTakeawayAPI.GraphQL.Order;
using Xunit;

namespace WeDoTakeawayAPI.GraphQL.Tests.Orders
{
    public class AddOrderMutationTests : BaseTests
    {
        private async Task<dynamic> AddOrderForItem(IEnumerable<Guid> itemIds, int quantity = 1)
        {
            var items = itemIds.Select(itemId => new OrderItemInput(itemId, quantity)).ToList();

            var addOrderInput = new AddOrderInput(
                "Fred",
                "Address1",
                "Address2",
                "Town",
                "Postcode",
                "1234",
                "Fred@fred.com",
                "Under the mat",
                Guid.NewGuid(),
                items
            );


            IExecutionResult result = await ServiceProvider.ExecuteRequestAsync(
                QueryRequestBuilder
                    .New()
                    .SetQuery(@"
                        mutation AddOrder($input: AddOrderInput!) {
                          addOrder(input: $input) {
                            order {
                              id
                              createdAt
                            }
                            errors {
                              code
                              message
                            }
                          }
                        }
                    ")
                    .SetVariableValue(name: "input", value: addOrderInput)
                    .Create()
            );

            var json = await result.ToJsonAsync();

            Assert.NotNull(json);

            ExpandoObject response = JsonConvert.DeserializeObject<ExpandoObject>(json, new ExpandoObjectConverter());

            Assert.NotNull(response);

            return response;
        }

        [Fact]
        public async Task Add_Two_Valid_Items()
        {
            ApplicationDbContext dbContext = GetDbContext();

            var itemIds = new List<Guid>
            {
                Guid.Parse("600DCA30-C6E2-4035-AD15-783C122D6EA4"),
                Guid.Parse("600DCA30-C6E2-4035-AD15-783C122D6EA5")
            };
            dynamic response = await AddOrderForItem(itemIds);

            // Make sure there were no errors
            Assert.Null(response.data.addOrder.errors);

            // Check the response includes an order id
            Assert.True(Guid.TryParse(response.data.addOrder.order.id as string, out _));
            var orderId = Guid.Parse(response.data.addOrder.order.id as string ?? string.Empty);

            // Fetch the order created in the database
            Model.Order order = await dbContext.Orders
                .Where(o => o.Id == orderId)
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync();

            Assert.NotNull(order);

            Assert.Equal(2, order.OrderItems.Count);

            OrderItem firstItem = order.OrderItems.First();

            Assert.Equal("Plate of sausages", firstItem.Name);
            Assert.Equal("Big bowl of sausages", firstItem.Description);
            Assert.Equal(Guid.Parse("600DCA30-C6E2-4035-AD15-783C122D6EA4"), firstItem.ItemId);
            Assert.Equal(1, firstItem.Quantity);

            // Check that the second item was correctly added
            OrderItem secondItem = order.OrderItems.ElementAt(1);

            Assert.Equal("Chocolate ice-cream surprise", secondItem.Name);
            Assert.Equal("An amazing mixture of chocolate and cherries", secondItem.Description);
            Assert.Equal(Guid.Parse("600DCA30-C6E2-4035-AD15-783C122D6EA5"), secondItem.ItemId);
            Assert.Equal(1, secondItem.Quantity);
        }

        [Fact]
        public async Task Add_Invalid_Item_Id()
        {
            var itemIds = new List<Guid>
            {
                Guid.Parse("600DCA30-C6E2-4035-AD15-783C122D6EA4"),
                Guid.Parse("700DCA30-C6E2-4035-AD15-783C122D6EA4")
            };
            dynamic response = await AddOrderForItem(itemIds);

            // Look for the error
            Assert.NotNull(response.data.addOrder.errors);
            Assert.Equal("1012", response.data.addOrder.errors[0].code);
            Assert.Equal("Invalid order item id", response.data.addOrder.errors[0].message);
        }

        [Fact]
        public async Task Add_Too_Few_Items()
        {
            var itemIds = new List<Guid>
            {
                Guid.Parse("600DCA30-C6E2-4035-AD15-783C122D6EA4"),
            };
            dynamic response = await AddOrderForItem(itemIds, 0);

            // Look for the error
            Assert.NotNull(response.data.addOrder.errors);
            Assert.Equal("1011", response.data.addOrder.errors[0].code);
            Assert.Equal("Invalid order item quantity", response.data.addOrder.errors[0].message);
        }

        [Fact]
        public async Task Add_Too_Many_Items()
        {
            var itemIds = new List<Guid>
            {
                Guid.Parse("600DCA30-C6E2-4035-AD15-783C122D6EA4"),
            };
            dynamic response = await AddOrderForItem(itemIds, 102);

            // Look for the error
            Assert.NotNull(response.data.addOrder.errors);
            Assert.Equal("1011", response.data.addOrder.errors[0].code);
            Assert.Equal("Invalid order item quantity", response.data.addOrder.errors[0].message);
        }

        [Fact]
        public async Task Add_Too_Empty_Item_Array()
        {
            dynamic response = await AddOrderForItem(new List<Guid>());

            // Look for the error
            Assert.NotNull(response.data.addOrder.errors);
            Assert.Equal("1010", response.data.addOrder.errors[0].code);
            Assert.Equal("No items to order", response.data.addOrder.errors[0].message);
        }
    }
}
