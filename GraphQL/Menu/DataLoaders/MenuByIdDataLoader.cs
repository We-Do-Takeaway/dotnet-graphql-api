using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut;
using HotChocolate.DataLoader;
using Microsoft.EntityFrameworkCore;
using WeDoTakeawayAPI.GraphQL.Model;

namespace WeDoTakeawayAPI.GraphQL.Menu.DataLoaders
{
    public class MenuByIdDataLoader : BatchDataLoader<Guid, Model.Menu>
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

        public MenuByIdDataLoader(
            IBatchScheduler batchScheduler,
            IDbContextFactory<ApplicationDbContext> dbContextFactory)
            : base(batchScheduler)
        {
            _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
        }

        protected override async Task<IReadOnlyDictionary<Guid, Model.Menu>> LoadBatchAsync(
            IReadOnlyList<Guid> keys,
            CancellationToken cancellationToken)
        {
            await using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

            return await dbContext.Menus.Where(m => keys.Contains(m.Id))
                .ToDictionaryAsync(t => t.Id, cancellationToken);
        }
    }
}