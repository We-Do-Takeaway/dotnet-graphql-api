using System.Threading.Tasks;
using HotChocolate.Execution;
using Snapshooter.Xunit;
using Xunit;

namespace WeDoTakeawayAPI.GraphQL.Tests
{
    public class SectionTests : BaseTests
    {
        [Fact]
        public async Task Get_Section_By_Id()
        {
            var result = await ServiceProvider.ExecuteRequestAsync(
                QueryRequestBuilder
                    .New()
                    .SetQuery(@"
                        query SectionById($id: ID!) {
                          sectionById(id: $id) {
                            id
                            name
                          }
                        }")
                    .SetVariableValue(name: "id", value: "600dca30-c6e2-4035-ad15-783c122d6ea3")
                    .Create());
        
            result.MatchSnapshot();
        }

        [Fact]
        public async Task Get_Section_Items()
        {
            var result = await ServiceProvider.ExecuteRequestAsync(
                QueryRequestBuilder
                    .New()
                    .SetQuery(@"
                        query SectionById($id: ID!) {
                          sectionById(id: $id) {
                            id
                            name
                            items {
                              nodes {
                                id
                                name
                                description
                              } 
                            }
                          }
                        }")
                    .SetVariableValue(name: "id", value: "600dca30-c6e2-4035-ad15-783c122d6ea3")
                    .Create());
        
            result.MatchSnapshot();
        }
        
        [Fact]
        public async Task Get_Section_Menu()
        {
            var result = await ServiceProvider.ExecuteRequestAsync(
                QueryRequestBuilder
                    .New()
                    .SetQuery(@"
                        query SectionById($id: ID!) {
                          sectionById(id: $id) {
                            id
                            name
                            menu {
                              id
                              name
                              description
                            }
                          }
                        }")
                    .SetVariableValue(name: "id", value: "600dca30-c6e2-4035-ad15-783c122d6ea3")
                    .Create());
        
            result.MatchSnapshot();
        }
    }
}