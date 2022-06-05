#region

using FluentValidation;

#endregion

namespace Application.Books.Commands.UpdateBook;

public class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
{
    public UpdateBookCommandValidator()
    {
        RuleFor(v => v.Author)
            .Matches(@"^[a-zA-Z ]+$")
            .When(a => a.Author != "")
            .WithMessage("Author name should only contain alphabet characters and spaces");
        RuleFor(v => v.ISBN)
            .Matches(@"^[0-9\-]+$")
            .When(a => a.ISBN != "")
            .WithMessage("ISBN should only contain numbers and dash");
        RuleFor(v => v.Price)
            .Must(p => p is -1 or > 0);
        RuleFor(v => v.Stock)
            .GreaterThanOrEqualTo(-1);
    }
}