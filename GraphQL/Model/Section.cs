using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HotChocolate;

namespace WeDoTakeawayAPI.GraphQL.Model
{
    [Table("section")]
    public class Section
    {
        [Column("id")]
        public Guid Id { get; set; }
        
        [Required]
        [Column("menu_id")]
        [GraphQLIgnore]
        public Guid? MenuId { get; set; }
        
        [GraphQLIgnore]
        public Menu? Menu { get; set; }
        
        [Required]
        [Column("name")]
        [StringLength(100)]
        public string? Name { get; set; }
        
        [Column("description")]
        public string? Description { get; set; }
        
        [Column("introduction")]
        public string? Introduction { get; set;  }
        
        [Column("footer")]
        public string? Footer { get; set; }
        
        [Column("photo")]
        [StringLength(100)]
        public string? Photo { get; set; }
        
        [Column("display_order")]
        public int? DisplayOrder { get; set; }
        
        public ICollection<SectionItem> SectionItems { get; set; } = 
            new List<SectionItem>();
    }
}