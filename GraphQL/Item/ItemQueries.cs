using System;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Types;

namespace WeDoTakeawayAPI.GraphQL.Item
{
    [ExtendObjectType(Name = "Query")]
    public class ItemQueries
    {
        public Task<Model.Item> GetItemByIdAsync(
            Guid id, 
            ItemByIdDataLoader dataLoader, 
            CancellationToken cancellationToken) => 
            dataLoader.LoadAsync(id, cancellationToken);
    }
}