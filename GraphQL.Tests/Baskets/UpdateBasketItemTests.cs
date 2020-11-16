using System;
using System.Dynamic;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Execution;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using WeDoTakeawayAPI.GraphQL.Baskets;
using Xunit;

namespace WeDoTakeawayAPI.GraphQL.Tests.Baskets
{
    public class UpdateBasketItemsTests:BaseBasketTests
    {
        private async Task<dynamic> UpdateBasketItem(Guid basketId, Guid itemId, int qty)
        {
            var basketItemInput = new BasketItemInput(basketId, itemId,  qty);

            IExecutionResult result = await ServiceProvider.ExecuteRequestAsync(
                QueryRequestBuilder
                    .New()
                    .SetQuery(@"
                        mutation UpdateBasketItem($input: BasketItemInput!) {
                          updateBasketItem(input: $input) {
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
                    .SetVariableValue(name: "input", value: basketItemInput)
                    .Create()
            );

    
            // Check against the snapshot, the existing basket was returned
            var json = await result.ToJsonAsync();

            Assert.NotNull(json);
            
            var response = JsonConvert.DeserializeObject<ExpandoObject>(json, new ExpandoObjectConverter());
            
            Assert.NotNull(response);

            return response;
        }
        
        // When the client updates an existing basket item
        [Fact]
        public async Task Update_Existing_Item_In_Basket()
        {
            var basketId = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            var itemId = Guid.NewGuid();
            
            await SaveBasket(basketId,customerId, itemId);

            dynamic response = await UpdateBasketItem(basketId, itemId, 5);
            
            // Check that errors is empty
            Assert.Null(response.data.updateBasketItem.errors);
            
            // Now validate that the item has been updated
            Assert.Equal(1, response.data.updateBasketItem.basket.items.Count);
            Assert.Equal(5, response.data.updateBasketItem.basket.items[0].qty);
        }
        
        // When the client updates a basket item that is not in the basket
        [Fact]
        public async Task Update_Invalid_Item()
        {
            var basketId = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            var itemId = Guid.NewGuid();
            var badItemId = Guid.NewGuid();
            
            await SaveBasket(basketId,customerId, itemId);

            dynamic response = await UpdateBasketItem(basketId, badItemId, 2);
            
            // Check that errors object has an entry
            Assert.NotNull(response.data.updateBasketItem.errors);
            
            // Check that the basket item is null
            Assert.Null(response.data.updateBasketItem.basket);
            
            // Now check we have the correct error
            Assert.Equal("1002", response.data.updateBasketItem.errors[0].code);
            Assert.Equal("Invalid Basket Item ID", response.data.updateBasketItem.errors[0].message);
        }
        
        // When the client updates a basket item in a basket that doesn't exist
        [Fact]
        public async Task Update_Invalid_Basket()
        {
            var basketId = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            var itemId = Guid.NewGuid();
            var badBasketId = Guid.NewGuid();
            
            await SaveBasket(basketId,customerId, itemId);

            dynamic response = await UpdateBasketItem(badBasketId, itemId, 2);
            
            // Check that errors object has an entry
            Assert.NotNull(response.data.updateBasketItem.errors);
            
            // Check that the basket item is null
            Assert.Null(response.data.updateBasketItem.basket);
            
            // Now check we have the correct error
            Assert.Equal("1001", response.data.updateBasketItem.errors[0].code);
            Assert.Equal("Invalid Basket ID", response.data.updateBasketItem.errors[0].message);
        }
        
        // Set item to qty 0, remove it
        [Fact]
        public async Task Set_Qty_To_Zero()
        {
            var basketId = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            var itemId = Guid.NewGuid();
            
            await SaveBasket(basketId,customerId, itemId);
            
            dynamic response = await UpdateBasketItem(basketId, itemId, 0);
            
            // Check that errors is empty
            Assert.Null(response.data.updateBasketItem.errors);
            
            // validate that the item has been updated in the response
            Assert.Equal(0, response.data.updateBasketItem.basket.items.Count);
        }
    }
}