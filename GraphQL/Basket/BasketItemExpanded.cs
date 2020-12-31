using System;
using System.ComponentModel.DataAnnotations.Schema;
using HotChocolate;
using HotChocolate.Types;

namespace WeDoTakeawayAPI.GraphQL.Basket
{
    [GraphQLName("BasketItem")]
    public class BasketItemExpanded
    {
        public BasketItemExpanded(
            Guid id,
            int quantity,
            string name,
            string? description,
            string? photo
        )
        {
            Id = id;
            Quantity = quantity;
            Name = name;
            Description = description;
            Photo = photo;
        }

        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? Photo { get; set; }

        public int? Quantity { get; }
    }

    public class BasketItemExpandedType : ObjectType<BasketItemExpanded>
    {

    }
}
