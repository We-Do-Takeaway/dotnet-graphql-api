using System;
using System.Dynamic;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Execution;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using WeDoTakeawayAPI.GraphQL.Basket;
using Xunit;

namespace WeDoTakeawayAPI.GraphQL.Tests.Baskets.Mutations
{
    public class UpdateBasketItemsTests:BaseBasketTests
    {
        private async Task<dynamic> UpdateBasketItem(Guid ownerId, Guid itemId, int quantity)
        {
            var basketItemInput = new BasketItemInput(ownerId, itemId, quantity);

            IExecutionResult result = await ServiceProvider.ExecuteRequestAsync(
                QueryRequestBuilder
                    .New()
                    .SetQuery(@"
                        mutation UpdateBasketItem($input: BasketItemInput!) {
                          updateBasketItem(input: $input) {
                            basket {
                              id
                              items {
                                id
                                quantity
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
            var basketId = Guid.Parse("8d404353-ecb6-4aee-8a54-ff85e0f7332a");
            var ownerId = Guid.Parse("0ead61d8-dc83-4530-9518-7acbbf090824");
            var itemId = Guid.Parse("600dca30-c6e2-4035-ad15-783c122d6ea4"); // plate of sausages

            await CreateBasketWithItem(basketId, ownerId, itemId);

            dynamic response = await UpdateBasketItem(ownerId, itemId, 5);

            // Check that errors is empty
            Assert.Null(response.data.updateBasketItem.errors);

            // Now validate that the item has been updated
            Assert.Equal(1, response.data.updateBasketItem.basket.items.Count);
            Assert.Equal(5, response.data.updateBasketItem.basket.items[0].quantity);
        }

        // When the client updates a basket item that is not in the basket
        [Fact]
        public async Task Update_Invalid_Item()
        {
            var basketId = Guid.Parse("8d404353-ecb6-4aee-8a54-ff85e0f7332a");
            var ownerId = Guid.Parse("0ead61d8-dc83-4530-9518-7acbbf090824");
            var itemId = Guid.Parse("600dca30-c6e2-4035-ad15-783c122d6ea4"); // plate of sausages
            var badItemId = Guid.NewGuid();

            await CreateBasketWithItem(basketId, ownerId, itemId);


            dynamic response = await UpdateBasketItem(ownerId, badItemId, 2);

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
            var basketId = Guid.Parse("8d404353-ecb6-4aee-8a54-ff85e0f7332a");
            var ownerId = Guid.Parse("0ead61d8-dc83-4530-9518-7acbbf090824");
            var itemId = Guid.Parse("600dca30-c6e2-4035-ad15-783c122d6ea4"); // plate of sausages
            var badOwnerId = Guid.NewGuid();

            await CreateBasketWithItem(basketId, ownerId, itemId);

            dynamic response = await UpdateBasketItem(badOwnerId, itemId, 2);

            // Check that errors object has an entry
            Assert.NotNull(response.data.updateBasketItem.errors);

            // Check that the basket item is null
            Assert.Null(response.data.updateBasketItem.basket);

            // Now check we have the correct error
            Assert.Equal("1001", response.data.updateBasketItem.errors[0].code);
            Assert.Equal("Invalid basket owner ID", response.data.updateBasketItem.errors[0].message);
        }

        // Set item to quantity 0, remove it
        [Fact]
        public async Task Set_Quantity_To_Zero()
        {
            var basketId = Guid.Parse("8d404353-ecb6-4aee-8a54-ff85e0f7332a");
            var ownerId = Guid.Parse("0ead61d8-dc83-4530-9518-7acbbf090824");
            var itemId = Guid.Parse("600dca30-c6e2-4035-ad15-783c122d6ea4"); // plate of sausages

            await CreateBasketWithItem(basketId, ownerId, itemId);

            dynamic response = await UpdateBasketItem(ownerId, itemId, 0);

            // Check that errors is empty
            Assert.Null(response.data.updateBasketItem.errors);

            // validate that the item has been updated in the response
            Assert.Equal(0, response.data.updateBasketItem.basket.items.Count);
        }
    }
}
