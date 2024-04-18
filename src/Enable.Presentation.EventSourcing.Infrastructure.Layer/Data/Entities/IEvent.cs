using System;

namespace Enable.Presentation.EventSourcing.Infrastructure.Layer.Data.Entities;

public interface IEvent
{
    public long SequenceNumber { get; set; }

    public string Name { get; set; }

    public string Payload { get; set; }

    public DateTimeOffset EnqueuedDateTime { get; set; }
}
