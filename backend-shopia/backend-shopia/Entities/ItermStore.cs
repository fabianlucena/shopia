using RFService.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RFService.Attributes;

namespace backend_shopia.Entities
{
    [Table("ItemsStores", Schema = "shopia")]
    public class ItemStore
        : EntityCreatedAt
    {
        [Required]
        [ForeignKey("Item")]
        public Int64 ItemId { get; set; } = default;
        public Item? Item { get; set; } = default;

        [Required]
        [ForeignKey("Store")]
        public Int64 StoreId { get; set; } = default;
        public Store? Store { get; set; } = default;

        [Virtual]
        public Commerce? Commerce { get; set; }
    }
}