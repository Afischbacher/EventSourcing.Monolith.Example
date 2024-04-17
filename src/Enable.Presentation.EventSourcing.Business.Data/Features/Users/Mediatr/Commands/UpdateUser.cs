using Enable.Presentation.EventSourcing.DataAccess.Layer.Repositories;
using Enable.Presentation.EventSourcing.Infrastructure.Layer.Data.Entities;
using MediatR;

namespace Enable.Presentation.EventSourcing.Business.Layer.Features.Users.Mediatr.Commands
{
    /// <summary>
    /// A mediator command to retrieve a user by their user id
    /// </summary>
    public class UpdateUser : IRequest<User?>
    {
        public required Guid UserId { get; set; }
    }

    /// <summary>
    /// The handler for the GetUser command to retrieve a user by their user id
    /// </summary>
    public class UpdateUserHandler(IUsersRepository usersRepository) : IRequestHandler<UpdateUser, User?>
    {
        private readonly IUsersRepository _usersRepository = usersRepository;

        public async Task<User?> Handle(UpdateUser request, CancellationToken cancellationToken)
        {
            var userId = request.UserId;

            var user = await _usersRepository.GetByUserIdAsync(userId);
            if (user == null)
            {
                return null;
            }

            _usersRepository.Update(user);
            await _usersRepository.SaveAsync(cancellationToken);

            return user;
        }
    }
}
