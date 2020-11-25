using System.Threading.Tasks;
using HotChocolate.Execution;
using Snapshooter.Xunit;
using Xunit;

namespace WeDoTakeawayAPI.GraphQL.Tests
{
    public class ItemTests : BaseTests
    {
        [Fact]
        public async Task Get_Item_By_Id()
        {
            var result = await ServiceProvider.ExecuteRequestAsync(
                QueryRequestBuilder
                    .New()
                    .SetQuery(@"
                        query ItemById($id: ID!) {
                          itemById(id: $id) {
                            id
                            name
                          }
                        }")
                    .SetVariableValue(name: "id", value: "600dca30-c6e2-4035-ad15-783c122d6ea6")
                    .Create());
        
            result.MatchSnapshot();
        }

        [Fact]
        public async Task Get_Item_Sections()
        {
            var result = await ServiceProvider.ExecuteRequestAsync(
                QueryRequestBuilder
                .New()
                .SetQuery(@"
                    query ItemById($id: ID!) {
                      itemById(id:$id) {
                        id
                        name
                        sections {
                          id
                          name
                        }
                      }
                    }")
                .SetVariableValue(name: "id", value: "600dca30-c6e2-4035-ad15-783c122d6ea6")
                .Create());
        
            result.MatchSnapshot();
        }
        
        [Fact]
        public async Task Get_Item_Ingredients()
        {
            var result = await ServiceProvider.ExecuteRequestAsync(
                QueryRequestBuilder
                    .New()
                    .SetQuery(@"
                    query ItemById($id: ID!) {
                      itemById(id:$id) {
                        id
                        name
                        ingredients {
                            id
                            name
                            quantity
                        }
                      }
                    }")
                    .SetVariableValue(name: "id", value: "600dca30-c6e2-4035-ad15-783c122d6ea6")
                    .Create());
        
            result.MatchSnapshot();
        }
    }
}