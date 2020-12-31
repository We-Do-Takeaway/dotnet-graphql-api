using System;
using HotChocolate;
using HotChocolate.Types;

namespace WeDoTakeawayAPI.GraphQL.Ingredient
{

    [GraphQLName("ItemIngredient")]
    public class ItemIngredientWithQuantity
    {
        public ItemIngredientWithQuantity(
            Guid id,
            int? quantity,
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

        public Guid Id { get; }
        public string? Name { get; }
        public string? Description { get; }
        public string? Photo { get; }
        public int? Quantity { get; }
    }

    public class ItemIngredientWithQuantityType : ObjectType<ItemIngredientWithQuantity>
    {
    }
}
