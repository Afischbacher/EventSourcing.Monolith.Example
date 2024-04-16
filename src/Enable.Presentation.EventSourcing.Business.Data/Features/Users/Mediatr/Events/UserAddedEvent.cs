using Enable.Presentation.EventSourcing.DataAccess.Layer.Repositories;
using Enable.Presentation.EventSourcing.Infrastructure.Layer.Data.Entities;
using MediatR;
using System.Text.Json;

namespace Enable.Presentation.EventSourcing.Business.Layer.Features.Users.Mediatr.Events
{
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
    public class UserAddedEventHandler(IEventRepository eventRepository) : INotificationHandler<UserAddedEvent>
    {
        private readonly IEventRepository _eventRepository = eventRepository;

        public async Task Handle(UserAddedEvent notification, CancellationToken cancellationToken)
        {
            await _eventRepository.AddAsync(new Event
            {
                Name = nameof(UserAddedEvent),
                Payload = JsonSerializer.Serialize(notification),
                EnqueuedDateTime = DateTimeOffset.UtcNow
            });
        }
    }
}
