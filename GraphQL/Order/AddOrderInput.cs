using System;
using System.Collections.Generic;

namespace WeDoTakeawayAPI.GraphQL.Order
{
    public record OrderItemInput(
        Guid ItemId,
        int Quantity
    );

    public record AddOrderInput(
        string Name,
        string Address1,
        string? Address2,
        string Town,
        string Postcode,
        string Phone,
        string? Email,
        string? DeliveryInstructions,
        Guid OwnerId,
        IEnumerable<OrderItemInput> Items
    );
}
