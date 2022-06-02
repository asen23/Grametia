#region

using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

#endregion

namespace Application.Users.Commands.CreateUser;

public record CreateUserCommand : IRequest<long>
{
    public string Username { get; init; } = default!;
    public string Address { get; init; } = default!;
    public string PhoneNumber { get; init; } = default!;
    public string Email { get; init; } = default!;
}

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, long>
{
    private readonly IApplicationDbContext _context;

    public CreateUserCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<long> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var entity = new User
        {
            Username = request.Username,
            Address = request.Address,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Role = "member"
        };

        entity.Cart = new Cart();

        _context.Users.Add(entity);
        entity.Cart.Items.Add(new CartItem
        {
            Amount = 1,
            Book = new Book
            {
                Title = "tes",
                Edition = "tes",
                Description = "tes",
                Author = "tes",
                Publisher = "tes",
                ISBN = "tes",
                Category = "tes",
                ReleaseDate = "tes",
                Price = 1,
                Stock = 1
            }
        });

        await _context.SaveChangesAsync(cancellationToken);


        return entity.Id;
    }
}