using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Enable.Presentation.EventSourcing.Infrastructure.Layer.Data.Entities;


[PrimaryKey(nameof(SequenceNumber))]
[Index(nameof(SequenceNumber), IsUnique = true)]
[Table("Events", Schema = "dbo")]
public class Event : IEntity
{
    [Key]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long SequenceNumber { get; set; }

    [MaxLength(128)]
    [Required]
    public required string Name { get; set; }

    [MaxLength(int.MaxValue)]
    [Required]
    public required string Payload { get; set; }

    public required DateTimeOffset EnqueuedDateTime { get; set; } = DateTimeOffset.UtcNow;
}
