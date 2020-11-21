using System;

namespace WeDoTakeawayAPI.GraphQL.Basket
{
    public record BasketItemDeleteInput (
        Guid BasketId,
        Guid ItemId
    );
}