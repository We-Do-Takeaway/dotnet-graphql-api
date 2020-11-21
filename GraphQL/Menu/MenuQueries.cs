using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using WeDoTakeawayAPI.GraphQL.Extensions;
using WeDoTakeawayAPI.GraphQL.Menu.DataLoaders;
using WeDoTakeawayAPI.GraphQL.Model;

namespace WeDoTakeawayAPI.GraphQL.Menu
{
    [ExtendObjectType(Name = "Query")]
    public class MenuQueries
    {
        [UseApplicationDbContext]
        [UsePaging]
        public Task<List<Model.Menu>> GetMenus(
            [ScopedService] ApplicationDbContext context) => 
            context.Menus.OrderBy(m => m.Name).ToListAsync();
     
        public Task<Model.Menu> GetMenuByIdAsync(
            Guid id, 
            MenuByIdDataLoader dataLoader, 
            CancellationToken cancellationToken) => 
            dataLoader.LoadAsync(id, cancellationToken);
    }
}