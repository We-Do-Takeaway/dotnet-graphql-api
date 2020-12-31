using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore.Infrastructure;
using WeDoTakeawayAPI.GraphQL.Section.DataLoaders;

namespace WeDoTakeawayAPI.GraphQL.Section
{
    [ExtendObjectType(Name = "Query")]
    public class SectionQueries
    {
        public async Task<Model.Section> GetSectionByIdAsync(
            Guid id,
            SectionByIdDataLoader dataLoader,
            CancellationToken cancellationToken)
        {
            Model.Section section = await dataLoader.LoadAsync(id, cancellationToken);

            if (section == null)
            {
                var extensions = new Dictionary<string, object?>() {
                    { "code", "9001" },
                    { "id", id}
                };

                Error error = new("Invalid id", extensions: extensions);
                throw new QueryException(error);
            }

            return section;

        }


    }
}
