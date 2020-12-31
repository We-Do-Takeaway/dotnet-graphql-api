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
    public class RemoveBasketItemTests:BaseBasketTests
    {
        private async Task<dynamic> RemoveBasketItem(Guid ownerId, Guid itemId)
        {
            var basketItemDeleteInput = new BasketItemDeleteInput(ownerId, itemId);

            IExecutionResult result = await ServiceProvider.ExecuteRequestAsync(
                QueryRequestBuilder
                    .New()
                    .SetQuery(@"
                        mutation RemoveBasketItem($input: BasketItemDeleteInput!) {
                          removeBasketItem(input: $input) {
                            basket {
                              id
                              ownerId
                              items {
                                id
                                quantity
                              }
                            }
                          }
                        }
                    ")
                    .SetVariableValue(name: "input", value: basketItemDeleteInput)
                    .Create()
            );


            // Check against the snapshot, the existing basket was returned
            var json = await result.ToJsonAsync();

            Assert.NotNull(json);

            ExpandoObject response = JsonConvert.DeserializeObject<ExpandoObject>(json, new ExpandoObjectConverter());

            Assert.NotNull(response);

            return response;
        }

        // When there is 1 item and the client removes it
        [Fact]
        public async Task Remove_Only_Basket_Item()
        {
            var basketId = Guid.Parse("8d404353-ecb6-4aee-8a54-ff85e0f7332a");
            var ownerId = Guid.Parse("0ead61d8-dc83-4530-9518-7acbbf090824");
            var itemId = Guid.Parse("600dca30-c6e2-4035-ad15-783c122d6ea4"); // plate of sausages

            await CreateBasketWithItem(basketId, ownerId, itemId);

            dynamic response = await RemoveBasketItem(ownerId, itemId);

            // Now validate that the item has been updated
            Assert.Equal(0, response.data.removeBasketItem.basket.items.Count);
        }

        // When there are 2 items and the client removes one
        [Fact]
        public async Task Remove_Basket_Item()
        {
            var basketId = Guid.Parse("8d404353-ecb6-4aee-8a54-ff85e0f7332a");
            var ownerId = Guid.Parse("0ead61d8-dc83-4530-9518-7acbbf090824");

            // Plate of sausages
            var itemId1 = Guid.Parse("600dca30-c6e2-4035-ad15-783c122d6ea4");

            // Bowl of cherries
            var itemId2 = Guid.Parse("600dca30-c6e2-4035-ad15-783c122d6ea6");

            // Create a basket with a plate of sausages
            await CreateBasketWithItem(basketId, ownerId, itemId1);

            // Add a bowl of cherries to the basket items
            await AddItem(basketId, itemId2);

            // Make a call to remove the plate of sausages
            dynamic response = await RemoveBasketItem(ownerId, itemId1);

            // Now validate that the item has been updated
            Assert.Equal(1, response.data.removeBasketItem.basket.items.Count);
            Assert.Equal(itemId2.ToString(), response.data.removeBasketItem.basket.items[0].id.ToString());
        }

        // When the item does not exist in the basket
        [Fact]
        public async Task Remove_Invalid_Item()
        {
            var basketId = Guid.Parse("8d404353-ecb6-4aee-8a54-ff85e0f7332a");
            var ownerId = Guid.Parse("0ead61d8-dc83-4530-9518-7acbbf090824");
            var itemId = Guid.Parse("600dca30-c6e2-4035-ad15-783c122d6ea4"); // plate of sausages
            var badItemId = Guid.NewGuid();

            await CreateBasketWithItem(basketId, ownerId, itemId);

            dynamic response = await RemoveBasketItem(ownerId, badItemId);

            // Check that errors object has an entry
            Assert.NotNull(response.errors);

            // Now check we have the correct error
            Assert.Equal("1002", response.errors[0].extensions.code);
            Assert.Equal("Invalid basket item id", response.errors[0].message);
        }

        // When the basket doesn't exist
        [Fact]
        public async Task Remove_With_Invalid_Basket()
        {
            var basketId = Guid.Parse("8d404353-ecb6-4aee-8a54-ff85e0f7332a");
            var ownerId = Guid.Parse("0ead61d8-dc83-4530-9518-7acbbf090824");
            var itemId = Guid.Parse("600dca30-c6e2-4035-ad15-783c122d6ea4"); // plate of sausages
            var badOwnerId = Guid.NewGuid();

            await CreateBasketWithItem(basketId, ownerId, itemId);

            dynamic response = await RemoveBasketItem(badOwnerId, itemId);

            // Check that errors object has an entry
            Assert.NotNull(response.errors);

            // Now check we have the correct error
            Assert.Equal("1001", response.errors[0].extensions.code);
        }
    }
}
