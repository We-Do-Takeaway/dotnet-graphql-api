using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeDoTakeawayAPI.GraphQL.Model
{
    [Table("section_item")]
    public class SectionItem
    {
        [Required]
        [Column("section_id")]
        public Guid SectionId { get; set; }
        
        public Section? Section { get; set; }
        
        [Required]
        [Column("item_id")]
        public Guid ItemId { get; set; }
        
        public Item? Item { get; set; }
        
        [Column("display_order")]
        public int? DisplayOrder { get; set; }
    }
}