using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HotChocolate;

namespace WeDoTakeawayAPI.GraphQL.Model
{
    [Table("basket")]
    public class Basket
    {
        [Column("id")]
        public Guid Id { get; set; }
        
        [Required]
        [Column("customer_id")]
        public Guid CustomerId { get; set; }

        public ICollection<BasketItem> Items { get; set; } =
            new List<BasketItem>();
    }
}


