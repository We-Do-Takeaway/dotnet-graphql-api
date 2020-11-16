using System;
using System.Dynamic;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Execution;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Xunit;

namespace WeDoTakeawayAPI.GraphQL.Tests.Baskets
{
    public class BasketQueriesTests:BaseBasketTests
    {
        private async Task<dynamic> GetBasket(Guid customerId)
        {
            IExecutionResult result = await ServiceProvider.ExecuteRequestAsync(
                QueryRequestBuilder
                    .New()
                    .SetQuery(@"
                        query BasketByCustomerId($id: ID!) {
                            basketByCustomerId(id: $id) {
                                id
                                customerId
                                items {
                                    id
                                    itemId
                                    qty
                                }
                            }
                        }
                    ")
                    .SetVariableValue(name: "id", value: customerId.ToString())
                    .Create()
            );
            
            // Check against the snapshot, the existing basket was returned
            var json = await result.ToJsonAsync();

            Assert.NotNull(json);
            
            // Deserialize the response so we can test it
            dynamic response = JsonConvert.DeserializeObject<ExpandoObject>(json, new ExpandoObjectConverter());
            Assert.NotNull(response);

            return response;
        }
        
        // When a query is made for an existing basket
        [Fact]
        public async Task Get_Existing_Customer_Basket()
        {
            var basketId = Guid.NewGuid();
            var customerId = Guid.NewGuid();

            await SaveBasket(basketId, customerId);

            var response = await GetBasket(customerId);
            
            Assert.True(Guid.TryParse(response.data.basketByCustomerId.id as string, out _));
            Assert.Equal(customerId.ToString(), response.data.basketByCustomerId.customerId);
            Assert.Equal(0, response.data.basketByCustomerId.items.Count); 
        }

        [Fact]
        public async Task Get_Existing_With_Items()
        {
            var basketId = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            var itemId = Guid.NewGuid();
            
            await SaveBasket(basketId, customerId, itemId);

            var response = await GetBasket(customerId);
            
            Assert.Equal(1, response.data.basketByCustomerId.items.Count);
            Assert.True(Guid.TryParse(response.data.basketByCustomerId.items[0].id as string, out _));
            Assert.Equal(itemId.ToString(), response.data.basketByCustomerId.items[0].itemId);
            Assert.Equal(1, response.data.basketByCustomerId.items[0].qty);
        }

        // When a query is made for a customer with no basket
        [Fact]
        public async Task Get_New_Customer_Basket()
        {
            var customerId = Guid.NewGuid();
            var response = await GetBasket(customerId);

            // Verify that the customer id for the basket is correct and that a
            // new basket with a valid Guid was created, with no items
            Assert.Equal(customerId.ToString(), response.data.basketByCustomerId.customerId);
            Assert.True(Guid.TryParse(response.data.basketByCustomerId.id as string, out _));
            Assert.Equal(0,response.data.basketByCustomerId.items.Count);
        }
       
    }
}