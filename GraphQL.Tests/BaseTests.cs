using System;
using System.IO;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WeDoTakeawayAPI.GraphQL.Basket;
using WeDoTakeawayAPI.GraphQL.Item.DataLoaders;
using WeDoTakeawayAPI.GraphQL.Menu;
using WeDoTakeawayAPI.GraphQL.Menu.DataLoaders;
using WeDoTakeawayAPI.GraphQL.Model;
using WeDoTakeawayAPI.GraphQL.Section;
using WeDoTakeawayAPI.GraphQL.Section.DataLoaders;

namespace WeDoTakeawayAPI.GraphQL.Tests
{
    public class BaseTests
    {
        protected IServiceProvider ServiceProvider { get; }
        
        protected BaseTests()
        {
            var dbFileName = this.GetType().Name;

            if (File.Exists(dbFileName))
            {
                File.Delete(dbFileName);
            }

            ServiceProvider = new ServiceCollection()
                .AddPooledDbContextFactory<ApplicationDbContext>
                    (options => options.UseSqlite($"Data Source={dbFileName}"))
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
                .Services
                .BuildServiceProvider();

            using var scope = ServiceProvider.CreateScope();
            var factory = scope.ServiceProvider.GetService<IDbContextFactory<ApplicationDbContext>>();
            if (factory == null) return;
            using var dbContext = factory.CreateDbContext();
            dbContext.Database.Migrate();
        }
        
        protected ApplicationDbContext GetDbContext()
        {
            using var scope = ServiceProvider.CreateScope();
            var factory = scope.ServiceProvider.GetService<IDbContextFactory<ApplicationDbContext>>();
            var dbContext = factory?.CreateDbContext();
            return dbContext;
        }
    }
}