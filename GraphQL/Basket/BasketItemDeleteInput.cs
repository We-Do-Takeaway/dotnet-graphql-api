using System;

namespace WeDoTakeawayAPI.GraphQL.Baskets
{
    public record BasketItemDeleteInput (
        Guid BasketId,
        Guid ItemId
    );
}