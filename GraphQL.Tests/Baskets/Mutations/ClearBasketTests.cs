using System;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Execution;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using WeDoTakeawayAPI.GraphQL.Model;
using Xunit;


namespace WeDoTakeawayAPI.GraphQL.Tests.Baskets.Mutations
{
    public class ClearBasketTests:BaseBasketTests
    {
        private async Task<dynamic> ClearBasket(Guid ownerId)
        {
            IExecutionResult result = await ServiceProvider.ExecuteRequestAsync(
                QueryRequestBuilder
                    .New()
                    .SetQuery(@"
                        mutation ClearBasketByOwnerId($id: ID!) {
                          clearBasketByOwnerId(id: $id) {
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
                    .SetVariableValue(name: "id", value: ownerId.ToString())
                    .Create()
            );


            // Check against the snapshot, the existing basket was returned
            var json = await result.ToJsonAsync();

            Assert.NotNull(json);

            ExpandoObject response = JsonConvert.DeserializeObject<ExpandoObject>(json, new ExpandoObjectConverter());

            Assert.NotNull(response);

            return response;
        }

        // Clear a valid basket with items in it
        [Fact]
        public async Task Clear_Valid_Basket_Response()
        {
            var basketId = Guid.Parse("8d404353-ecb6-4aee-8a54-ff85e0f7332a");
            var ownerId = Guid.Parse("0ead61d8-dc83-4530-9518-7acbbf090824");
            var itemId = Guid.Parse("600dca30-c6e2-4035-ad15-783c122d6ea4"); // plate of sausages

            await CreateBasketWithItem(basketId, ownerId, itemId);

            dynamic response = await ClearBasket(ownerId);

            // Now validate that we have a clear basket
            dynamic basket = response.data.clearBasketByOwnerId.basket;
            Assert.Equal(basketId.ToString(), basket.id);
            Assert.Equal(ownerId.ToString(), basket.ownerId);
            Assert.Equal(0, basket.items.Count);
        }

        // Clear a valid basket with no items in it
        [Fact]
        public async Task Clear_Empty_Basket_Response()
        {
            var basketId = Guid.Parse("8d404353-ecb6-4aee-8a54-ff85e0f7332a");
            var ownerId = Guid.Parse("0ead61d8-dc83-4530-9518-7acbbf090824");

            await CreateEmptyBasket(basketId,  ownerId);

            dynamic response = await ClearBasket(ownerId);

            // Now validate that we have a clear basket
            dynamic basket = response.data.clearBasketByOwnerId.basket;
            Assert.Equal(basketId.ToString(), basket.id);
            Assert.Equal(ownerId.ToString(), basket.ownerId);
            Assert.Equal(0, basket.items.Count);
        }

        [Fact]
        public async Task clear_Empty_Basket_DB()
        {
            var basketId = Guid.Parse("8d404353-ecb6-4aee-8a54-ff85e0f7332a");
            var ownerId = Guid.Parse("0ead61d8-dc83-4530-9518-7acbbf090824");

            await CreateEmptyBasket(basketId ,  ownerId);

            await ClearBasket(ownerId);

            ApplicationDbContext dbContext = GetDbContext();
            Model.Basket basket = await dbContext
                .Baskets.Where(b => b.Id == basketId)
                .Include(b => b.BasketItems)
                .FirstOrDefaultAsync();

            Assert.Equal(ownerId, basket.OwnerId);
            Assert.Equal(0, basket.BasketItems.Count);
        }

        // Clear an invalid basket
        [Fact]
        public async Task Clear_Invalid_Basket_Response()
        {
            var badOwnerId = Guid.NewGuid();

            dynamic response = await ClearBasket(badOwnerId);

            // Check that errors object has an entry
            Assert.NotNull(response.errors);

            // Now check we have the correct error
            Assert.Equal("1001", response.errors[0].extensions.code);
            Assert.Equal("Invalid basket owner id", response.errors[0].message);
        }
    }
}
