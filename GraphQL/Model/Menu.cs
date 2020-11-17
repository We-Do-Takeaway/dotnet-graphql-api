using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeDoTakeawayAPI.GraphQL.Model
{
    [Table("menu")]
    public class Menu
    {
        [Column("id")]
        public Guid Id { get; set; }
        
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
        
        public ICollection<Section> Sections { get; set; } =
            new List<Section>();
    }
}