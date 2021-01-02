using System;
using System.Threading.Tasks;
using HotChocolate.Execution;
using Snapshooter.Xunit;
using WeDoTakeawayAPI.GraphQL.Model;
using Xunit;

namespace WeDoTakeawayAPI.GraphQL.Tests.Orders
{
    public class OrderQueriesTests: BaseTests
    {
        private async Task<Model.Order> CreateOrder()
        {
            ApplicationDbContext dbContext = GetDbContext();

            var orderId = Guid.Parse("56c1f9c7-54a5-4f3d-b967-1f4073415d08");

            var order = new Model.Order
            {
                Id = orderId,
                Name = "Fred",
                Address1 = "ad1",
                Address2 = "ad2",
                Town = "town",
                Postcode = "pcode",
                Phone = "1234",
                Email = "fred@d.d",
                DeliveryInstructions = "In the coal bunker",
                OwnerId = Guid.Parse("9940c08a-34b5-4e42-a736-bf9aec716f09"),
                CreatedAt = DateTime.Parse("08/18/2018 07:22:16")
            };

            var orderItem = new OrderItem
            {
                OrderId = orderId,
                ItemId = Guid.Parse("600DCA30-C6E2-4035-AD15-783C122D6EA4"),
                Quantity = 1,
                Name = "Plate of sausages",
                Description = "Big bowl of sausages"
            };

            await dbContext.Orders.AddAsync(order);
            await dbContext.OrderItems.AddAsync(orderItem);
            await dbContext.SaveChangesAsync();

            return order;
        }

        [Fact]
        public async Task Get_Order_For_Id()
        {
            Model.Order order = await CreateOrder();

            IExecutionResult result = await ServiceProvider.ExecuteRequestAsync(
                QueryRequestBuilder
                    .New()
                    .SetQuery(@"
                        query OrderById($id:ID!) {
	                        orderById(id: $id) {
                            id
                            name
                            address1
                            address2
                            town
                            postcode
                            phone
                            email
                            deliveryInstructions
                            items {
                              itemId
                              name
                              description
                              quantity
                            }
                          }
                        }
                    ")
                    .SetVariableValue(name: "id", value: order.Id.ToString())
                    .Create());

            result.MatchSnapshot();
        }

        [Fact]
        public async Task Get_Orders()
        {
            Model.Order order = await CreateOrder();

            IExecutionResult result = await ServiceProvider.ExecuteRequestAsync(
                QueryRequestBuilder
                    .New()
                    .SetQuery(@"
                        query Orders {
	                        orders {
                                id
                                name
                                address1
                                address2
                                town
                                postcode
                                phone
                                email
                                deliveryInstructions
                                items {
                                  itemId
                                  name
                                  description
                                  quantity
                                }
                            }
                        }
                    ")
                    .Create());

            result.MatchSnapshot();
        }
    }
}
