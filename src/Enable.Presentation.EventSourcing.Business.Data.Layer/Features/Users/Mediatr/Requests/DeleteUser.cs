using Enable.Presentation.EventSourcing.DataAccess.Layer.Repositories;
using MediatR;

namespace Enable.Presentation.EventSourcing.Business.Layer.Features.Users.Mediatr.Requests;

/// <summary>
/// A mediator command to delete a user by their user id
/// </summary>
public class DeleteUser : IRequest<bool>
{
    public required Guid UserId { get; set; }
}

/// <summary>
/// The handler for the DeleteUser command to delete a user by their user id
/// </summary>
/// <param name="usersRepository">The user repostiory</param>
public class DeleteUserHandler(IUsersRepository usersRepository) : IRequestHandler<DeleteUser, bool>
{
    private readonly IUsersRepository _usersRepository = usersRepository;

    public async Task<bool> Handle(DeleteUser request, CancellationToken cancellationToken)
    {
        var userId = request.UserId;

        var user = await _usersRepository.GetByUserIdAsync(userId);
        if (user == null)
        {
            return false;
        }

        _usersRepository.Delete(user);
        await _usersRepository.SaveAsync(cancellationToken);

        return true;
    }
}
