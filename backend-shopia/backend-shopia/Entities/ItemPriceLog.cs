using RFService.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RFService.Attributes;

namespace backend_shopia.Entities
{
    [Table("ItemsPricesLog", Schema = "shopia")]
    public class ItemPriceLog
        : EntityCreatedAt
    {
        [Required]
        [ForeignKey("Item")]
        public Int64 ItemId { get; set; } = default;
        public Item? Item { get; set; } = default;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
    }
}