using RFService.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_shopia.Entities
{
    [Table("Plans", Schema = "shopia")]
    public class Plan
        : EntitySoftDeleteTimestampsIdUuidEnabledName
    {
        [ForeignKey("ExtendTo")]
        public Int64? ExtendToId { get; set; } = default;
        public Plan? ExtendTo { get; set; } = default;

        public string? Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
    }
}