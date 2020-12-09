using System;

namespace WeDoTakeawayAPI.GraphQL.Basket
{
    public record BasketItemInput(
        Guid OwnerId,
        Guid ItemId,
        int Quantity
    );
}
