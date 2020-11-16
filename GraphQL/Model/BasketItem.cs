using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HotChocolate;

namespace WeDoTakeawayAPI.GraphQL.Model
{
    [Table("basket_item")]
    public class BasketItem
    {
        [Column("id")]
        public Guid Id { get; set; }
        
        [Required]
        [Column("item_id")]
        public Guid ItemId { get; set; }
        
        [Required]
        [Column("qty")]
        public int Qty { get; set; }
        
        [Required]
        [Column("basket_id")]
        [GraphQLIgnore]
        public Guid BasketId { get; set; }

        [GraphQLIgnore]
        public Basket? Basket { get; set; }
    }
}