namespace Domain.Entities;

public class User : BaseEntity
{
    public string Username { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public string Email { get; set; } = default!;

    public Cart Cart { get; set; } = default!;
}