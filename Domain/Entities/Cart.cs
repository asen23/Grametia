namespace Domain.Entities;

public class Cart : BaseEntity
{
    public long UserId { get; set; } = default!;
    public User User { get; set; } = default!;

    public List<CartItem> Items { get; set; } = default!;
}