using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WeDoTakeawayAPI.GraphQL.Baskets;
using WeDoTakeawayAPI.GraphQL.Model;
using WeDoTakeawayAPI.GraphQL.DataLoaders;

namespace WeDoTakeawayAPI.GraphQL.Tests.Baskets
{
    public abstract class BaseBasketTests
    {
        protected IServiceProvider ServiceProvider { get; }

        protected BaseBasketTests()
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
                .AddMutationType(d => d.Name("Mutation"))
                .AddTypeExtension<BasketMutations>()
                .AddDataLoader<ItemByIdDataLoader>()
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

        protected async Task SaveBasket(Guid basketId, Guid customerId, Guid? itemId = null, int qty = 1)
        {
            var dbContext = GetDbContext();

            var basket = new Basket {Id = basketId, CustomerId = customerId};

            if (itemId != null)
            {
                var item = new BasketItem
                {
                    BasketId = basketId,
                    ItemId = (Guid) itemId,
                    Qty = 1
                };

                basket.Items.Add(item);
            }
            await dbContext.Baskets.AddAsync(basket);
            await dbContext.SaveChangesAsync();
        }

        protected async Task AddItem(Guid basketId, Guid itemId, int qty = 1)
        {
            var item2 = new BasketItem
            {
                BasketId = basketId,
                ItemId = itemId,
                Qty = 1
            };

            var dbContext = GetDbContext();

            Basket basket = await dbContext.Baskets.FindAsync(basketId);
            basket.Items.Add(item2);
            await dbContext.SaveChangesAsync();
        }
    }
}