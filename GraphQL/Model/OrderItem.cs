using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HotChocolate;

namespace WeDoTakeawayAPI.GraphQL.Model
{
    [Table("order_item")]
    public class OrderItem
    {
        [Required]
        [Column("item_id")]
        public Guid ItemId { get; set; }

        public Item? Item { get; set; }

        [Required]
        [Column("order_id")]
        [GraphQLIgnore]
        public Guid OrderId { get; set; }

        public Order? Order { get; set; }

        [Required]
        [Column("name")]
        [StringLength(100)]
        public string? Name { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Required]
        [Column("quantity")]
        public int Quantity { get; set; }
    }
}
