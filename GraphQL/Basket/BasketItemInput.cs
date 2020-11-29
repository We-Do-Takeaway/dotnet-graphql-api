using System;

namespace WeDoTakeawayAPI.GraphQL.Basket
{
    public record BasketItemInput(
        Guid BasketId,
        Guid ItemId,
        int Quantity
    );
}
