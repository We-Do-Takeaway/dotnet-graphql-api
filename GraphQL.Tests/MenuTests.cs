using System.Threading.Tasks;
using HotChocolate.Execution;
using Snapshooter.Xunit;
using Xunit;

namespace WeDoTakeawayAPI.GraphQL.Tests
{
    public class MenuTests : BaseTests
    {
        [Fact]
        public async Task Get_Menus()
        {
            var result = await ServiceProvider.ExecuteRequestAsync(
                QueryRequestBuilder
                    .New()
                    .SetQuery(@"
                        query Menus {
                          menus {
                            nodes {
                              id
                              name
                              sections {
                                nodes {
                                  id
                                  name
                                  displayOrder
                                }
                              }
                            }
                         }
                        }")
                    .Create());
        
           result.MatchSnapshot();
            
        }
        
        [Fact]
        public async Task Get_Menu_By_Id()
        {
            var result = await ServiceProvider.ExecuteRequestAsync(
                QueryRequestBuilder
                    .New()
                    .SetQuery(@"
                        query MenuById($id: ID!) {
                          menuById(id: $id) {
                            id
                            name
                          }
                        }")
                    .SetVariableValue(name: "id", value: "600dca30-c6e2-4035-ad15-783c122d6ea1")
                    .Create());
        
            result.MatchSnapshot();
            
        }

        [Fact]
        public async Task Get_Sections_InMenu()
        {
            var result = await ServiceProvider.ExecuteRequestAsync(
                QueryRequestBuilder
                    .New()
                    .SetQuery(@"
                        query MenuWithSections($id: ID!) {
                          menuById(id: $id) {
                            id
                            name
                            sections {
                              nodes {
                                id
                                name
                                displayOrder
                              }
                            }
                          }
                        }")
                    .SetVariableValue(name: "id", value: "600dca30-c6e2-4035-ad15-783c122d6ea1")
                    .Create());
        
            result.MatchSnapshot();
        } 
    }
}