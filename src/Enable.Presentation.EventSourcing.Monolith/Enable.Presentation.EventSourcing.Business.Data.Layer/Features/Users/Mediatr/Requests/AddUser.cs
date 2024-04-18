using Enable.Presentation.EventSourcing.Business.Layer.Features.Users.Mediatr.Events;
using Enable.Presentation.EventSourcing.DataAccess.Layer.Repositories;
using Enable.Presentation.EventSourcing.Infrastructure.Layer.Data.Entities;
using FluentValidation;
using MediatR;

namespace Enable.Presentation.EventSourcing.Business.Layer.Features.Users.Mediatr.Requests;

public class AddUser : IRequest<User>
{
    public required User User { get; set; }
}

public class AddUserHandler(IUsersRepository usersRepository, IMediator mediator, IValidator<User> validator) : IRequestHandler<AddUser, User>
{
    private readonly IUsersRepository _usersRepository = usersRepository;
    private readonly IMediator _mediator = mediator;
    private readonly IValidator<User> _validator = validator;

    public async Task<User> Handle(AddUser request, CancellationToken cancellationToken)
    {
        var user = request.User;
        await _validator.ValidateAndThrowAsync(user, cancellationToken);

        await _usersRepository.AddAsync(new User
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber
        });

        await _mediator.Publish(new UserAddedEvent
        {
            EmailAddress = user.Email
        }, cancellationToken);

        await _usersRepository.SaveAsync(cancellationToken);

        return user;
    }
}
