using Enable.Presentation.EventSourcing.DataAccess.Layer.Repositories;
using Enable.Presentation.EventSourcing.Infrastructure.Layer.Data.Entities;
using MediatR;
using System.Text.Json;

namespace Enable.Presentation.EventSourcing.Business.Layer.Features.Users.Mediatr.Events;

/// <summary>
/// An event for when a user is added to the system 
/// </summary>
public class UserAddedEvent : INotification
{
    public required string EmailAddress { get; set; }
}

/// <summary>
/// The event handler for the UserAddedEvent
/// </summary>
public class UserAddedEventHandler(IEventRepository eventRepository, IEventOutBoxRepository eventOutBoxRepository) : INotificationHandler<UserAddedEvent>
{
    private readonly IEventRepository _eventRepository = eventRepository;
    private readonly IEventOutBoxRepository _eventOutBoxRepository = eventOutBoxRepository;

    public async Task Handle(UserAddedEvent notification, CancellationToken cancellationToken)
    {
        var @event = new Event
        {
            Name = nameof(UserAddedEvent),
            Payload = JsonSerializer.Serialize(notification),
            EnqueuedDateTime = DateTimeOffset.UtcNow
        };

        await _eventRepository.AddAsync(@event);
        await _eventOutBoxRepository.AddAsync(EventOutBox.MapFromEvent(@event));
    }
}
