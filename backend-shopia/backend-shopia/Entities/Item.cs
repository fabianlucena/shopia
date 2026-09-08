using RFEntities.Attributes;
using RFEntities.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_shopia.Entities;

[Table("Items", Schema = "shopia")]
public class Item
    : ANominableEntity
{
    [Required]
    public bool InheritedIsActive { get; set; }

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    [ForeignKey("Category")]
    public long CategoryId { get; set; } = default;
    public Category? Category { get; set; } = default;

    [Virtual]
    public IEnumerable<ItemStore>? ItemsStores { get; set; } = default;

    [Required]
    [ForeignKey("Commerce")]
    public long CommerceId { get; set; } = default;
    public Commerce? Commerce { get; set; } = default;

    [Virtual]
    public IEnumerable<Store>? Stores { get; set; } = default;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    public int? Stock { get; set; }

    [Required]
    public bool IsPresent { get; set; } = false;

    public int? MinAge { get; set; }

    public int? MaxAge { get; set; }

    [Required]
    [Size(384)]
    public float[] Embedding { get; set; } = [];

    public Item() { }

    public Item(Item data)
        : base(data)
    {
        if (data == null)
            return;

        InheritedIsActive = data.InheritedIsActive;
        Description = data.Description;
        CategoryId = data.CategoryId;
        CommerceId = data.CommerceId;
        Price = data.Price;
        Stock = data.Stock;
        IsPresent = data.IsPresent;
        MinAge = data.MinAge;
        MaxAge = data.MaxAge;
        Embedding = (float[])data.Embedding.Clone();
    }

    public override Item Clone()
        => new(this);
}