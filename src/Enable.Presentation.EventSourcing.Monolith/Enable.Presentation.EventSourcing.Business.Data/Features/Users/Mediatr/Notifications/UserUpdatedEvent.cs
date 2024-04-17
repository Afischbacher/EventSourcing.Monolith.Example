using Enable.Presentation.EventSourcing.DataAccess.Layer.Repositories;
using Enable.Presentation.EventSourcing.Infrastructure.Layer.Data.Entities;
using MediatR;
using System.Text.Json;

namespace Enable.Presentation.EventSourcing.Business.Layer.Features.Users.Mediatr.Events;

/// <summary>
/// An event for when a user is updated in the system
/// </summary>
public class UserUpdatedEvent : INotification
{
    public required User User { get; set; }
}

/// <summary>
/// An event for when a user is updated in the system
/// </summary>
public class UserUpdatedEventHandler(IEventRepository eventRepository, IEventOutBoxRepository eventOutBoxRepository) : INotificationHandler<UserUpdatedEvent>
{
    private readonly IEventRepository _eventRepository = eventRepository;
    private readonly IEventOutBoxRepository _eventOutBoxRepository = eventOutBoxRepository;

    public async Task Handle(UserUpdatedEvent notification, CancellationToken cancellationToken)
    {
        var @event = new Event
        {
            Name = nameof(UserUpdatedEvent),
            Payload = JsonSerializer.Serialize(notification),
            EnqueuedDateTime = DateTimeOffset.UtcNow
        };

        await _eventRepository.AddAsync(@event);
        await _eventOutBoxRepository.AddAsync(EventOutBox.MapFromEvent(@event));
    }
}
