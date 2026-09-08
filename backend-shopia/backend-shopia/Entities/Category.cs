using RFEntities.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_shopia.Entities;

[Table("Categories", Schema = "shopia")]
public class Category
    : ANominableEntity
{
    [Required]
    public string Description { get; set; } = string.Empty;

    public Category() { }

    public Category(Category? category)
        : base(category)
    {
        if (category is null)
            return;

        this.Description = category.Description;
    }

    public override Category Clone()
        => new(this);
}