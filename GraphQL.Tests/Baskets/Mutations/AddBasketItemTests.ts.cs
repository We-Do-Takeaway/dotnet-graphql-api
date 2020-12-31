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
    public class AddBasketItemTests:BaseBasketTests
    {
        private async Task<dynamic> AddItemToBasket(Guid ownerId, Guid itemId, int quantity)
        {
            var basketItemInput = new BasketItemInput(ownerId, itemId, quantity);

            IExecutionResult result = await ServiceProvider.ExecuteRequestAsync(
                QueryRequestBuilder
                    .New()
                    .SetQuery(@"
                        mutation AddBasketItem($input: BasketItemInput!) {
                          addBasketItem(input: $input) {
                            basket {
                              id
                              ownerId
                              items {
                                id
                                name
                                quantity
                              }
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

            ExpandoObject response = JsonConvert.DeserializeObject<ExpandoObject>(json, new ExpandoObjectConverter());

            Assert.NotNull(response);

            return response;
        }

        // When the client adds a new item to an existing basket, the item is added with the specified quantity
        [Fact]
        public async Task Add_NewItem_To_Basket()
        {
            var basketId = Guid.Parse("8d404353-ecb6-4aee-8a54-ff85e0f7332a");
            var ownerId = Guid.Parse("0ead61d8-dc83-4530-9518-7acbbf090824");
            var itemId = Guid.Parse("600DCA30-C6E2-4035-AD15-783C122D6EA6"); // ice cream surprise

            await CreateEmptyBasket(basketId ,  ownerId);

            dynamic response = await AddItemToBasket(ownerId, itemId, 1);

            // Now validate that the item has been added
            Assert.True(Guid.TryParse(response.data.addBasketItem.basket.items[0].id as string, out _));
            Assert.Equal(itemId.ToString(), response.data.addBasketItem.basket.items[0].id);
            Assert.Equal(1, response.data.addBasketItem.basket.items[0].quantity);
        }

        // When the client adds an item to a basket that already has that item in it
        [Fact]
        public async Task Add_Existing_Item_To_Basket()
        {
            var basketId = Guid.Parse("8d404353-ecb6-4aee-8a54-ff85e0f7332a");
            var ownerId = Guid.Parse("0ead61d8-dc83-4530-9518-7acbbf090824");
            var itemId = Guid.Parse("600dca30-c6e2-4035-ad15-783c122d6ea4"); // plate of sausages

            await CreateBasketWithItem(basketId, ownerId, itemId);

            dynamic response = await AddItemToBasket(ownerId, itemId, 2);

            // Now validate that the item has been updated
            Assert.Equal(1, response.data.addBasketItem.basket.items.Count);
            Assert.Equal(3, response.data.addBasketItem.basket.items[0].quantity);
        }

        // When the client adds a new item to an a basket that does not exist
        [Fact]
        public async Task Add_NewItem_To_Invalid_Basket()
        {
            var badOwnerId = Guid.NewGuid();
            var itemId = Guid.Parse("600dca30-c6e2-4035-ad15-783c122d6ea4"); // plate of sausages

            dynamic response = await AddItemToBasket(badOwnerId, itemId, 1);

            // Check that errors object has an entry
            Assert.NotNull(response.errors);

            // How do I check the response code?

            // Now check we have the correct error
            Assert.Equal("1001", response.errors[0].extensions.code);
            Assert.Equal("Invalid basket owner id", response.errors[0].message);
        }

        // When quantity is set to 0 when adding an item, return an error
        [Fact]
        public async Task Add_Zero_Item_To_Basket()
        {
            var basketId = Guid.Parse("8d404353-ecb6-4aee-8a54-ff85e0f7332a");
            var ownerId = Guid.Parse("0ead61d8-dc83-4530-9518-7acbbf090824");
            var itemId = Guid.Parse("10114302-4328-495d-9831-9ec05465be47"); // ice cream surprise

            await CreateEmptyBasket(basketId ,  ownerId);

            dynamic response = await AddItemToBasket(ownerId, itemId, 0);

            // Check that errors object has an entry
            Assert.NotNull(response.errors);

            // Now check we have the correct error
            Assert.Equal("1003", response.errors[0].extensions.code);
            Assert.Equal("Invalid item quantity", response.errors[0].message);
        }
    }
}
