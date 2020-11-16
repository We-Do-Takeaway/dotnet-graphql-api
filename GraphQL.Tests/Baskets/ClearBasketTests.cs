using System;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Execution;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Xunit;


namespace WeDoTakeawayAPI.GraphQL.Tests.Baskets
{
    public class ClearBasketTests:BaseBasketTests
    {
        private async Task<dynamic> ClearBasket(Guid basketId)
        {
            IExecutionResult result = await ServiceProvider.ExecuteRequestAsync(
                QueryRequestBuilder
                    .New()
                    .SetQuery(@"
                        mutation ClearBasket($id: ID!) {
                          clearBasket(id: $id) {
                            basket {
                              id
                              customerId
                              items {
                                id
                                itemId
                                qty
                              }
                            }
                            errors {
                              code
                              message
                            }
                          }
                        }
                    ")
                    .SetVariableValue(name: "id", value: basketId.ToString())
                    .Create()
            );

    
            // Check against the snapshot, the existing basket was returned
            var json = await result.ToJsonAsync();

            Assert.NotNull(json);
            
            var response = JsonConvert.DeserializeObject<ExpandoObject>(json, new ExpandoObjectConverter());
            
            Assert.NotNull(response);

            return response;
        }
        
        // Clear a valid basket with items in it
        [Fact]
        public async Task Clear_Valid_Basket_Response()
        {
            var basketId = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            
            await SaveBasket(basketId,customerId, Guid.NewGuid());
            
            dynamic response = await ClearBasket(basketId);
            
            // Check that errors is empty
            Assert.Null(response.data.clearBasket.errors);
            
            // Now validate that we have a clear basket
            var basket = response.data.clearBasket.basket;
            Assert.Equal(basketId.ToString(), basket.id);
            Assert.Equal(customerId.ToString(), basket.customerId);
            Assert.Equal(0, basket.items.Count);
        }
        
        // Clear a valid basket with no items in it
        [Fact]
        public async Task Clear_Empty_Basket_Response()
        {
            var basketId = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            await SaveBasket(basketId,customerId);
            
            var response = await ClearBasket(basketId);
            
            // Check that errors is empty
            Assert.Null(response.data.clearBasket.errors);
            
            // Now validate that we have a clear basket
            var basket = response.data.clearBasket.basket;
            Assert.Equal(basketId.ToString(), basket.id);
            Assert.Equal(customerId.ToString(), basket.customerId);
            Assert.Equal(0, basket.items.Count);
        }
        
        [Fact]
        public async Task clear_Empty_Basket_DB()
        {
            var basketId = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            
            await SaveBasket(basketId,customerId);
            
            await ClearBasket(basketId);

            var dbContext = GetDbContext();
            var basket = await dbContext
                .Baskets.Where(b => b.Id == basketId)
                .Include(b => b.Items)
                .FirstOrDefaultAsync();
            
            Assert.Equal(customerId, basket.CustomerId);
            Assert.Equal(0, basket.Items.Count);
        }
        
        // Clear an invalid basket
        [Fact]
        public async Task Clear_Invalid_Basket_Response()
        {
            var basketId = Guid.NewGuid();
            var badBasketId = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            await SaveBasket(basketId,customerId);
            
            var response = await ClearBasket(badBasketId);
            
            // Check that errors object has an entry
            Assert.NotNull(response.data.clearBasket.errors);
            
            // Check that the basket item is null
            Assert.Null(response.data.clearBasket.basket);
            
            // Now check we have the correct error
            Assert.Equal("1001", response.data.clearBasket.errors[0].code);
            Assert.Equal("Invalid Basket ID", response.data.clearBasket.errors[0].message);
        }
    }
}