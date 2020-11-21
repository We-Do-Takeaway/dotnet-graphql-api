using System;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Types;
using WeDoTakeawayAPI.GraphQL.Section.DataLoaders;

namespace WeDoTakeawayAPI.GraphQL.Section
{
    [ExtendObjectType(Name = "Query")]
    public class SectionQueries
    {
        public Task<Model.Section> GetSectionByIdAsync(
            Guid id, 
            SectionByIdDataLoader dataLoader, 
            CancellationToken cancellationToken) => 
            dataLoader.LoadAsync(id, cancellationToken);
    }
}