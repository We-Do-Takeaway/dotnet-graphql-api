using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WeDoTakeawayAPI.GraphQL.Model;
using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Types;
using WeDoTakeawayAPI.GraphQL.Baskets;
using WeDoTakeawayAPI.GraphQL.DataLoaders;
using Snapshooter.Xunit;
using Xunit;

namespace WeDoTakeawayAPI.GraphQL.Tests.Baskets
{
    public class WeDoTakeawayAPISchemaTests:BaseBasketTests
    {
        [Fact]
        public async Task Verify_Schema()
        {
            ISchema schema = await new ServiceCollection()
                .AddPooledDbContextFactory<ApplicationDbContext>
                    (options => options.UseSqlite("Data Source=WeDoTakeawayAPISchemaTests.db"))
                .AddGraphQL()
                .BindRuntimeType<Guid, IdType>()
                .AddTypeConverter<Guid, string>(from => from.ToString("D"))
                .AddQueryType(d => d.Name("Query"))
                .AddTypeExtension<BasketQueries>()
                .AddMutationType(d => d.Name("Mutation"))
                .AddTypeExtension<BasketMutations>()
                .AddDataLoader<ItemByIdDataLoader>()
                .BuildSchemaAsync();

            // assert
            schema.Print().MatchSnapshot();
        }
    }
}