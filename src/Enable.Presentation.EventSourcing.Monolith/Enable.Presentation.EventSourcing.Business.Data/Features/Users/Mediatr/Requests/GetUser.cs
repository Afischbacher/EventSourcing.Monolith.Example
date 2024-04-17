using Enable.Presentation.EventSourcing.DataAccess.Layer.Repositories;
using Enable.Presentation.EventSourcing.Infrastructure.Layer.Data.Entities;
using MediatR;

namespace Enable.Presentation.EventSourcing.Business.Layer.Features.Users.Mediatr.Requests;

/// <summary>
/// A mediator command to retrieve a user by their user id
/// </summary>
public class GetUser : IRequest<User?>
{
    public required Guid UserId { get; set; }
}

/// <summary>
/// The handler for the GetUser command to retrieve a user by their user id
/// </summary>
/// <param name="usersRepository">The user repostiory</param>
public class GetUserHandler(IUsersRepository usersRepository) : IRequestHandler<GetUser, User?>
{
    private readonly IUsersRepository _usersRepository = usersRepository;

    public async Task<User?> Handle(GetUser request, CancellationToken cancellationToken)
    {
        var userId = request.UserId;

        return await _usersRepository.GetByUserIdAsync(userId);
    }
}
