using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Types;

namespace WeDoTakeawayAPI.GraphQL.Item
{
    [ExtendObjectType(Name = "Query")]
    public class ItemQueries
    {
        public async Task<Model.Item> GetItemByIdAsync(
            Guid id,
            ItemByIdDataLoader dataLoader,
            CancellationToken cancellationToken)
        {

            Model.Item item = await dataLoader.LoadAsync(id, cancellationToken);

            if (item == null)
            {
                var extensions = new Dictionary<string, object?>() {
                    { "code", "9001" },
                    { "id", id}
                };

                Error error = new("Invalid id", extensions: extensions);
                throw new QueryException(error);
            }

            return item;
        }
    }
}
