using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Enable.Presentation.EventSourcing.Infrastructure.Layer.Services.Messaging;

/// <summary>
/// A service to send messages to a Service Bus queue and Service Bus topics
/// </summary>
public interface IServiceBusMessagingService
{
    Task SendMessageAsync(string queueName, string? message = null);
}

/// <summary>
/// A service to send messages to a Service Bus queue and Service Bus topics
/// </summary>
public class ServiceBusMessagingService(IConfiguration configuration, ILogger<ServiceBusMessagingService> logger) : IServiceBusMessagingService, IAsyncDisposable
{
    private readonly ServiceBusClient _serviceBusClient = new(configuration.GetConnectionString("ServiceBusConnection"));
    private readonly ILogger<ServiceBusMessagingService> _logger = logger;

    /// <summary>
    /// Sends a message to a Service Bus queue or topic
    /// </summary>
    public async Task SendMessageAsync(string queueName, string? message = null)
    {
        var sender = _serviceBusClient.CreateSender(queueName);

        try
        {
            ServiceBusMessage serviceBusMessage = new(Encoding.UTF8.GetBytes(message ?? string.Empty));
            await sender.SendMessageAsync(serviceBusMessage);

        }
        catch (Exception exception)
        {
            _logger.LogError("Failed to send message to service Bus queue or topic", exception);
        }
        finally
        {
            await sender.DisposeAsync();
        }
    }

    /// <summary>
    /// Disposes of the Service Bus client when the service is disposed
    /// </summary>
    public ValueTask DisposeAsync()
    {
        if (_serviceBusClient != null)
        {
            return _serviceBusClient.DisposeAsync();
        }

        return default;
    }
}
