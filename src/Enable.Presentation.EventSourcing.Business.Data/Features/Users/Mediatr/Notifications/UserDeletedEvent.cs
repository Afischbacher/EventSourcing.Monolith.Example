using Enable.Presentation.EventSourcing.DataAccess.Layer.Repositories;
using Enable.Presentation.EventSourcing.Infrastructure.Layer.Data.Entities;
using MediatR;
using System.Text.Json;

namespace Enable.Presentation.EventSourcing.Business.Layer.Features.Users.Mediatr.Events
{
    /// <summary>
    /// An event for when a user is deleted from the system
    /// </summary>
    public class UserDeletedEvent : INotification
    {
        public required User User { get; set; }
    }

    /// <summary>
    /// An event for when a user is deleted from the system
    /// </summary>
    public class UserDeletedEventHandler(IEventRepository eventRepository) : INotificationHandler<UserDeletedEvent>
    {
        private readonly IEventRepository _eventRepository = eventRepository;

        public async Task Handle(UserDeletedEvent notification, CancellationToken cancellationToken)
        {
            await _eventRepository.AddAsync(new Event
            {
                Name = nameof(UserDeletedEvent),
                Payload = JsonSerializer.Serialize(notification.User),
                EnqueuedDateTime = DateTimeOffset.UtcNow
            });
        }
    }
}
