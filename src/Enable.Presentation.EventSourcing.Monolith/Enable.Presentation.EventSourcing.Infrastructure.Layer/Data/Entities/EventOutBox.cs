using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Enable.Presentation.EventSourcing.Infrastructure.Layer.Data.Entities;

[PrimaryKey(nameof(SequenceNumber))]
[Index(nameof(SequenceNumber), IsUnique = true)]
[Table("EventOutbox", Schema = "dbo")]
public class EventOutBox : IEntity, IEvent
{
    [Key]
    public required long SequenceNumber { get; set; }

    [MaxLength(128)]
    [Required]
    public required string Name { get; set; }

    [MaxLength(int.MaxValue)]
    [Required]
    public required string Payload { get; set; }

    public DateTimeOffset EnqueuedDateTime { get; set; } = DateTimeOffset.UtcNow;

    public static EventOutBox MapFromEvent(Event @event)
    {
        return new EventOutBox
        {
            Name = @event.Name,
            Payload = JsonSerializer.Serialize(@event.Payload),
            EnqueuedDateTime = @event.EnqueuedDateTime,
            SequenceNumber = @event.SequenceNumber
        };
    }
}
