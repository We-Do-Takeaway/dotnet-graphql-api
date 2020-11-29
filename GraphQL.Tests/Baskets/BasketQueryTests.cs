using System;
using System.Threading.Tasks;
using HotChocolate.Execution;
using Snapshooter.Xunit;
using Xunit;

namespace WeDoTakeawayAPI.GraphQL.Tests.Baskets
{
    public class BasketQueryTests : BaseBasketTests
    {
        [Fact]
        public async Task Get_New_Basket_For_Owner()
        {
            var result = await ServiceProvider.ExecuteRequestAsync(
                QueryRequestBuilder
                    .New()
                    .SetQuery(@"
                        query BasketByOwnerId($id: ID!) {
                            basketByOwnerId(id: $id) {
                                ownerId
                                basketType
                                items {
                                    id
                                    name
                                }
                            }
                        }
                    ")
                    .SetVariableValue(name: "id", value: "600dca30-c6e2-4035-ad15-783c122d6ea6")
                    .Create());

            result.MatchSnapshot();
        }

        [Fact]
        public async Task Get_Existing_Basket_For_Owner()
        {
            var basketId = Guid.Parse("8d404353-ecb6-4aee-8a54-ff85e0f7332a");
            var ownerId = Guid.Parse("0ead61d8-dc83-4530-9518-7acbbf090824");
            var itemId = Guid.Parse("600dca30-c6e2-4035-ad15-783c122d6ea4"); // plate of sausages

            await CreateBasketWithItem(basketId, ownerId, itemId);

            var result = await ServiceProvider.ExecuteRequestAsync(
                QueryRequestBuilder
                    .New()
                    .SetQuery(@"
                        query BasketByOwnerId($id: ID!) {
                            basketByOwnerId(id: $id) {
                                ownerId
                                basketType
                            }
                        }
                    ")
                    .SetVariableValue(name: "id", value: ownerId.ToString())
                    .Create());

            result.MatchSnapshot();
        }

        [Fact]
        public async Task Get_Item_Details()
        {
            var basketId = Guid.Parse("8d404353-ecb6-4aee-8a54-ff85e0f7332a");
            var ownerId = Guid.Parse("e46bbe60-c731-450d-8c44-f1f901ba0b24");
            var itemId = Guid.Parse("600dca30-c6e2-4035-ad15-783c122d6ea4"); // plate of sausages

            await CreateBasketWithItem(basketId, ownerId, itemId);

            var result = await ServiceProvider.ExecuteRequestAsync(
                QueryRequestBuilder
                    .New()
                    .SetQuery(@"
                        query BasketByOwnerId($id: ID!) {
                            basketByOwnerId(id: $id) {
                                ownerId
                                basketType
                                items {
                                    id
                                    name
                                }
                            }
                        }
                    ")
                    .SetVariableValue(name: "id", value: ownerId.ToString())
                    .Create());

            result.MatchSnapshot();
        }
    }
}
