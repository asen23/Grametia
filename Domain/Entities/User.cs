namespace Domain.Entities;

public class User : BaseEntity
{
    public User()
    {
        Cart = new Cart();
    }

    public string Username { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string Role { get; set; } = default!;

    public Cart Cart { get; set; }
}