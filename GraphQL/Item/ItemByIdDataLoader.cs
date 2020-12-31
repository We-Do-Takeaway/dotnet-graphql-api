using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut;
using HotChocolate.DataLoader;
using Microsoft.EntityFrameworkCore;
using WeDoTakeawayAPI.GraphQL.Model;

namespace WeDoTakeawayAPI.GraphQL.Item
{
    public class ItemByIdDataLoader : BatchDataLoader<Guid, Model.Item>
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

        public ItemByIdDataLoader(
            IBatchScheduler batchScheduler,
            IDbContextFactory<ApplicationDbContext> dbContextFactory)
            : base(batchScheduler)
        {
            _dbContextFactory = dbContextFactory ??
                                throw new ArgumentNullException(nameof(dbContextFactory));
        }

        protected override async Task<IReadOnlyDictionary<Guid, Model.Item>> LoadBatchAsync(
            IReadOnlyList<Guid> keys,
            CancellationToken cancellationToken)
        {
            await using ApplicationDbContext dbContext =
                _dbContextFactory.CreateDbContext();

            Dictionary<Guid, Model.Item>? result =  await dbContext.Items
                .Where(i => keys.Contains(i.Id))
                .ToDictionaryAsync(t => t.Id, cancellationToken);

            return result;
        }
    }
}
