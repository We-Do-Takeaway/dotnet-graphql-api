using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeDoTakeawayAPI.GraphQL.Model
{
    public enum BasketTypes
    {
        Anonymous,
        Customer
    }


    [Table("basket")]
    public class Basket
    {
        [Column("id")]
        public Guid Id { get; set; }

        [Required]
        [Column("owner_id")]
        public Guid OwnerId { get; set; }

        [Column("basket_type")]
        public BasketTypes BasketType { get; set; }

        public ICollection<BasketItem> BasketItems { get; set; } =
            new List<BasketItem>();
    }
}


