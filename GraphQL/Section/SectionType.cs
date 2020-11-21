using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Types;
using WeDoTakeawayAPI.GraphQL.Menu.DataLoaders;


namespace WeDoTakeawayAPI.GraphQL.Section
{
    public class SectionType : ObjectType<Model.Section>
    {
        protected override void Configure(IObjectTypeDescriptor<Model.Section> descriptor)
        {
            descriptor
                .Field(s => s.Menu)
                .ResolveWith<SectionResolvers>(t => 
                    t.GetMenuAsync(default!, default!, default!));
        }

        private class SectionResolvers
        {
            public async Task<Model.Menu?> GetMenuAsync(
                Model.Section section,
                MenuByIdDataLoader menuById,
                CancellationToken cancellationToken)
            {
                if (section.MenuId is null)
                {
                    return null;
                }

                return await menuById.LoadAsync(section.MenuId.Value, cancellationToken);
            }
        }
    }
}