using System;

namespace WeDoTakeawayAPI.GraphQL.Baskets
{
    public record BasketItemInput(
        Guid BasketId,
        Guid ItemId,
        int Qty
    );
}