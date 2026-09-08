using RFEntities.Attributes;
using RFEntities.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_shopia.Entities;

[Table("ItemsStores", Schema = "shopia")]
public class ItemStore
    : CreatableEntity
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

    public ItemStore() { }

    public ItemStore(ItemStore data)
        : base(data)
    {
        if (data == null)
            return;

        ItemId = data.ItemId;
        Item = data.Item;
        StoreId = data.StoreId;
        Store = data.Store;
        Commerce = data.Commerce;
    }

    public override ItemStore Clone()
        => new(this);
}