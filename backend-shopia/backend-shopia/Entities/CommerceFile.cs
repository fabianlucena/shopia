using RFEntities.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_shopia.Entities;

[Table("CommercesFiles", Schema = "shopia")]
public class CommerceFile
    : NominableEntity
{
    [Required]
    [ForeignKey("Commerce")]
    public Int64 CommerceId { get; set; } = default;
    public Commerce? Commerce { get; set; } = default;

    [Required]
    public string ContentType { get; set; } = "";

    [Required]
    public byte[] Content { get; set; } = [];

    public CommerceFile() { }

    public CommerceFile(CommerceFile data)
        : base(data)
    {
        if (data == null)
            return;

        CommerceId = data.CommerceId;
        Commerce = data.Commerce;
        ContentType = data.ContentType;
        Content = data.Content;
    }

    public override CommerceFile Clone()
        => new(this);
}