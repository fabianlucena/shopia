using RFEntities.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_shopia.Entities;

[Table("ItemsPricesLog", Schema = "shopia")]
public class ItemPriceLog
    : CreatableEntity
{
    [Required]
    [ForeignKey("Item")]
    public long ItemId { get; set; } = default;
    public Item? Item { get; set; } = default;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    public ItemPriceLog() { }

    public ItemPriceLog(ItemPriceLog data)
        : base(data)
    {
        if (data == null)
            return;

        ItemId = data.ItemId;
        Item = data.Item;
        Price = data.Price;
    }

    public override ItemPriceLog Clone()
        => new(this);
}