using System;
using System.Dynamic;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Execution;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using WeDoTakeawayAPI.GraphQL.Basket;
using Xunit;

namespace WeDoTakeawayAPI.GraphQL.Tests.Baskets
{
    public class AddBasketItemTests:BaseBasketTests
    {
        private async Task<dynamic> AddItemToBasket(Guid basketId, Guid itemId, int quantity)
        {
            var basketItemInput = new BasketItemInput(basketId, itemId, quantity);

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

        // When the client adds a new item to an existing basket, the item is added with the specified quantity
        [Fact]
        public async Task Add_NewItem_To_Basket()
        {
            var basketId = Guid.Parse("8d404353-ecb6-4aee-8a54-ff85e0f7332a");
            var ownerId = Guid.Parse("0ead61d8-dc83-4530-9518-7acbbf090824");
            var itemId = Guid.Parse("600DCA30-C6E2-4035-AD15-783C122D6EA6"); // ice cream surprise

            await CreateEmptyBasket(basketId ,  ownerId);

            dynamic response = await AddItemToBasket(basketId, itemId, 1);

            // Check that errors is empty
            Assert.Null(response.data.addBasketItem.errors);

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

            dynamic response = await AddItemToBasket(basketId, itemId, 2);

            // Check that errors is empty
            Assert.Null(response.data.addBasketItem.errors);

            // Now validate that the item has been updated
            Assert.Equal(1, response.data.addBasketItem.basket.items.Count);
            Assert.Equal(3, response.data.addBasketItem.basket.items[0].quantity);
        }

        // When the client adds a new item to an a basket that does not exist
        [Fact]
        public async Task Add_NewItem_To_Invalid_Basket()
        {
            var badBasketId = Guid.NewGuid();
            var itemId = Guid.Parse("600dca30-c6e2-4035-ad15-783c122d6ea4"); // plate of sausages

            dynamic response = await AddItemToBasket(badBasketId, itemId, 1);

            // Check that errors object has an entry
            Assert.NotNull(response.data.addBasketItem.errors);

            // Check that the basket item is null
            Assert.Null(response.data.addBasketItem.basket);

            // Now check we have the correct error
            Assert.Equal("1001", response.data.addBasketItem.errors[0].code);
            Assert.Equal("Invalid Basket ID", response.data.addBasketItem.errors[0].message);
        }

        // When quantity is set to 0 when adding an item, return an error
        [Fact]
        public async Task Add_Zero_Item_To_Basket()
        {
            var basketId = Guid.Parse("8d404353-ecb6-4aee-8a54-ff85e0f7332a");
            var ownerId = Guid.Parse("0ead61d8-dc83-4530-9518-7acbbf090824");
            var itemId = Guid.Parse("10114302-4328-495d-9831-9ec05465be47"); // ice cream surprise

            await CreateEmptyBasket(basketId ,  ownerId);

            dynamic response = await AddItemToBasket(basketId, itemId, 0);

            // Check that errors object has an entry
            Assert.NotNull(response.data.addBasketItem.errors);

            // Check that the basket item is null
            Assert.Null(response.data.addBasketItem.basket);

            // Now check we have the correct error
            Assert.Equal("1003", response.data.addBasketItem.errors[0].code);
            Assert.Equal("Invalid Item Quantity", response.data.addBasketItem.errors[0].message);
        }
    }
}
