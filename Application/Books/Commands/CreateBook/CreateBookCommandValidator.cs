#region

using FluentValidation;

#endregion

namespace Application.Books.Commands.CreateBook;

public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
{
    public CreateBookCommandValidator()
    {
        RuleFor(v => v.Title)
            .NotEmpty();
        RuleFor(v => v.Author)
            .NotEmpty()
            .Matches(@"^[a-zA-Z]+$")
            .WithMessage("Author name should only contain alphabet characters");
        RuleFor(v => v.Edition)
            .NotEmpty();
        RuleFor(v => v.Category)
            .NotEmpty();
        RuleFor(v => v.Publisher)
            .NotEmpty();
        RuleFor(v => v.ReleaseDate)
            .NotEmpty();
        RuleFor(v => v.ISBN)
            .NotEmpty()
            .Matches(@"^[0-9\-]+$")
            .WithMessage("ISBN should only contain numbers and dash");
        RuleFor(v => v.Description)
            .NotEmpty();
        RuleFor(v => v.Price)
            .GreaterThan(0);
        RuleFor(v => v.Stock)
            .GreaterThanOrEqualTo(0);
    }
}