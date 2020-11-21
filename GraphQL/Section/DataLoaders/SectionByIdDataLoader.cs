using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut;
using HotChocolate.DataLoader;
using Microsoft.EntityFrameworkCore;
using WeDoTakeawayAPI.GraphQL.Model;

namespace WeDoTakeawayAPI.GraphQL.Section.DataLoaders
{
    public class SectionByIdDataLoader : BatchDataLoader<Guid, Model.Section>
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

        public SectionByIdDataLoader(
            IBatchScheduler batchScheduler,
            IDbContextFactory<ApplicationDbContext> dbContextFactory) 
            : base(batchScheduler)
        {
            _dbContextFactory = dbContextFactory 
                                ?? throw new ArgumentNullException(nameof(dbContextFactory));
        }

        protected override async Task<IReadOnlyDictionary<Guid, Model.Section>> LoadBatchAsync(IReadOnlyList<Guid> keys,
            CancellationToken cancellationToken)
        {
            await using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

            return await dbContext.Sections
                .Where(s => keys.Contains(s.Id))
                .ToDictionaryAsync(t => t.Id, cancellationToken);
        }
    }
}