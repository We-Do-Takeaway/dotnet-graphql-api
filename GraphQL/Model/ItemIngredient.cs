using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeDoTakeawayAPI.GraphQL.Model
{
    [Table("item_ingredient")]
    public class ItemIngredient
    {
        [Required]
        [Column("item_id")]
        public Guid ItemId { get; set; }
        
        public Item? Item { get; set; }
        
        [Required]
        [Column("ingredient_id")]
        public Guid IngredientId { get; set; }
        
        public Ingredient? Ingredient { get; set; }
        
        [Required]
        [Column("quantity")]
        public int? Quantity { get; set; }
    }
}