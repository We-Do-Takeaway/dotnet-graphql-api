using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut;
using HotChocolate.DataLoader;
using Microsoft.EntityFrameworkCore;
using WeDoTakeawayAPI.GraphQL.Model;

namespace WeDoTakeawayAPI.GraphQL.Ingredient
{
    public class IngredientByIdDataLoader : BatchDataLoader<Guid, Model.Ingredient>
    {
    private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

    public IngredientByIdDataLoader(
        IBatchScheduler batchScheduler, 
        IDbContextFactory<ApplicationDbContext> dbContextFactory)
        : base(batchScheduler)
    {
        _dbContextFactory = dbContextFactory ?? 
                            throw new ArgumentNullException(nameof(dbContextFactory));
    }

    protected override async Task<IReadOnlyDictionary<Guid, Model.Ingredient>> LoadBatchAsync(
        IReadOnlyList<Guid> keys, 
        CancellationToken cancellationToken)
    {
        await using ApplicationDbContext dbContext = 
            _dbContextFactory.CreateDbContext();
            
        return await dbContext.Ingredients
            .Where(i => keys.Contains(i.Id))
            .ToDictionaryAsync(t => t.Id, cancellationToken);
    }
    }
}