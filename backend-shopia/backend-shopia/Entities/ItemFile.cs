using RFEntities.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_shopia.Entities;

[Table("ItemsFiles", Schema = "shopia")]
public class ItemFile
    : ACreatableWithNameEntity
{
    [Required]
    [ForeignKey("Item")]
    public long ItemId { get; set; } = default;
    public Item? Item { get; set; } = default;

    [Required]
    public string ContentType { get; set; } = "";

    [Required]
    public byte[] Content { get; set; } = [];

    public ItemFile() { }

    public ItemFile(ItemFile data)
        : base(data)
    {
        if (data == null)
            return;

        ItemId = data.ItemId;
        Item = data.Item;
        ContentType = data.ContentType;
        Content = data.Content;
    }

    public override ItemFile Clone()
        => new(this);
}