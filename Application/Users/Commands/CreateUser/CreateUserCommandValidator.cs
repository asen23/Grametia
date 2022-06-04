#region

using FluentValidation;

#endregion

namespace Application.Users.Commands.CreateUser;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(v => v.Username)
            .NotEmpty();
        RuleFor(v => v.Email)
            .NotEmpty()
            .EmailAddress();
        RuleFor(v => v.Password)
            .NotEmpty()
            .Matches(@"^(?=.*[a-z].*)(?=.*[A-Z].*)(?=.*[0-9].*)(?=.*[!@#$%^&*?].*).{8,}")
            .WithMessage(
                "Password should be at least 8 characters long and include uppercase, lowercase, number, and special character (!@#$%^&*?)");
        RuleFor(v => v.PhoneNumber)
            .NotEmpty()
            .Matches(@"^[0-9\-+]*$")
            .WithMessage("Phone number should only include numbers, dash and plus sign");
        RuleFor(v => v.Address)
            .NotEmpty();
        RuleFor(v => v.Role)
            .NotEmpty()
            .Must(r => r is "admin" or "member")
            .WithMessage("Role can only be filled with admin or member");
    }
}