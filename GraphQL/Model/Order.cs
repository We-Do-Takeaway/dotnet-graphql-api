using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeDoTakeawayAPI.GraphQL.Model
{
    [Table("order")]
    public class Order
    {
        [Column("id")]
        public Guid Id { get; set; }

        [Required]
        [Column("name")]
        [StringLength(100)]
        public string? Name { get; set; }

        [Required]
        [Column("address1")]
        [StringLength(100)]
        public string? Address1 { get; set; }

        [Column("address2")]
        [StringLength(100)]
        public string? Address2 { get; set; }

        [Required]
        [Column("town")]
        [StringLength(100)]
        public string? Town { get; set; }

        [Required]
        [Column("postcode")]
        [StringLength(8)]
        public string? Postcode { get; set; }

        [Required]
        [Column("phone")]
        [StringLength(20)]
        public string? Phone { get; set; }

        [Column("email")]
        [StringLength(100)]
        public string? Email { get; set; }

        [Required]
        [Column("delivery_instructions", TypeName = "text")]
        public string? DeliveryInstructions { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Required] [Column("owner_id")] public Guid OwnerId { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } =
            new List<OrderItem>();
    }
}
