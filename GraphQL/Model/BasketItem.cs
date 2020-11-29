using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HotChocolate;

namespace WeDoTakeawayAPI.GraphQL.Model
{
    [Table("basket_item")]
    public class BasketItem
    {
        [Required]
        [Column("item_id")]
        public Guid ItemId { get; set; }

        public Item? Item { get; set; }

        [Required]
        [Column("basket_id")]
        [GraphQLIgnore]
        public Guid BasketId { get; set; }

        public Basket? Basket { get; set; }

        [Required]
        [Column("quantity")]
        public int Quantity { get; set; }
    }
}
