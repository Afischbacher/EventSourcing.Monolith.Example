using Enable.Presentation.EventSourcing.Infrastructure.Layer.Data.Entities;
using FluentValidation;

namespace Enable.Presentation.EventSourcing.Business.Layer.Features.Users.Validators;

/// <summary>
/// A validator for the User entity when creating a user
/// </summary>
public class UserValidator : AbstractValidator<User>
{
    public UserValidator(CancellationToken cancellationToken = default)
    {
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.PhoneNumber).NotEmpty();
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
    }
}
