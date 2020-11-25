using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Snapshooter.Xunit;
using System;
using System.Threading.Tasks;
using WeDoTakeawayAPI.GraphQL.Basket;
using WeDoTakeawayAPI.GraphQL.Item;
using WeDoTakeawayAPI.GraphQL.Menu;
using WeDoTakeawayAPI.GraphQL.Menu.DataLoaders;
using WeDoTakeawayAPI.GraphQL.Model;
using WeDoTakeawayAPI.GraphQL.Section;
using WeDoTakeawayAPI.GraphQL.Section.DataLoaders;
using Xunit;

namespace WeDoTakeawayAPI.GraphQL.Tests
{
    public class WeDoTakeawayApiSchemaTests
    {
        [Fact]
        public async Task Verify_Schema()
        {
            var schema = await new ServiceCollection()
                .AddPooledDbContextFactory<ApplicationDbContext>
                    (options => options.UseSqlite("Data Source=WeDoTakeawayAPISchemaTests.db"))
                .AddGraphQL()
                .BindRuntimeType<Guid, IdType>()
                .AddTypeConverter<Guid, string>(from => from.ToString("D"))
                .AddQueryType(d => d.Name("Query"))
                .AddTypeExtension<BasketQueries>()
                .AddTypeExtension<MenuQueries>()
                .AddMutationType(d => d.Name("Mutation"))
                .AddTypeExtension<BasketMutations>()
                .AddType<MenuType>()
                .AddType<SectionType>()
                .AddDataLoader<ItemByIdDataLoader>()
                .AddDataLoader<MenuByIdDataLoader>()
                .AddDataLoader<SectionByIdDataLoader>()
                .BuildSchemaAsync();

            // assert
            schema.Print().MatchSnapshot();
        }
    }
}