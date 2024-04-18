using System.Runtime.Serialization;

namespace Enable.Presentation.EventSourcing.Business.Layer.Features.Users.Exceptions;

[Serializable]

/// <summary>
/// A custom exception for when a user is not found
/// </summary>
public class UserNotFoundException : Exception
{
    public UserNotFoundException()
    {
    }

    public UserNotFoundException(string? message) : base(message)
    {
    }

    public UserNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected UserNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}