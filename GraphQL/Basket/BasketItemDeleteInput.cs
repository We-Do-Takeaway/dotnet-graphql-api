using System;

namespace WeDoTakeawayAPI.GraphQL.Basket
{
    public record BasketItemDeleteInput (
        Guid OwnerId,
        Guid ItemId
    );
}
