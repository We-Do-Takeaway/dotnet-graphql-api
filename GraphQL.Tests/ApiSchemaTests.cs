using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Snapshooter.Xunit;
using System;
using System.Threading.Tasks;
using WeDoTakeawayAPI.GraphQL.Basket;
using WeDoTakeawayAPI.GraphQL.Basket.Mutations;
using WeDoTakeawayAPI.GraphQL.Ingredient;
using WeDoTakeawayAPI.GraphQL.Item;
using WeDoTakeawayAPI.GraphQL.Menu;
using WeDoTakeawayAPI.GraphQL.Menu.DataLoaders;
using WeDoTakeawayAPI.GraphQL.Model;
using WeDoTakeawayAPI.GraphQL.Order;
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
            ISchema schema = await new ServiceCollection()
                .AddPooledDbContextFactory<ApplicationDbContext>
                    (options => options.UseSqlite("Data Source=WeDoTakeawayAPISchemaTests.db"))
                .AddGraphQL()
                .BindRuntimeType<Guid, IdType>()
                .AddTypeConverter<Guid, string>(from => from.ToString("D"))
                .AddQueryType(d => d.Name("Query"))
                    .AddTypeExtension<BasketQueries>()
                    .AddTypeExtension<ItemQueries>()
                    .AddTypeExtension<MenuQueries>()
                    .AddTypeExtension<OrderQueries>()
                    .AddTypeExtension<SectionQueries>()
                .AddMutationType(d => d.Name("Mutation"))
                    .AddTypeExtension<AddBasketItemMutations>()
                    .AddTypeExtension<ClearBasketMutations>()
                    .AddTypeExtension<RemoveBasketItemMutations>()
                    .AddTypeExtension<UpdateBasketItemMutations>()
                .AddType<BasketType>()
                .AddType<ItemType>()
                .AddType<MenuType>()
                .AddType<SectionType>()
                .AddDataLoader<IngredientByIdDataLoader>()
                .AddDataLoader<ItemByIdDataLoader>()
                .AddDataLoader<MenuByIdDataLoader>()
                .AddDataLoader<SectionByIdDataLoader>()
                .AddFiltering()
                .AddSorting()
                .BuildSchemaAsync();

            // assert
            schema.Print().MatchSnapshot();
        }
    }
}
