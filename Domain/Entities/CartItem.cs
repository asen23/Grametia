namespace Domain.Entities;

public class CartItem : BaseEntity
{
    public int Amount { get; set; } = default!;
    public Book Book { get; set; } = default!;
    public Cart Cart { get; set; } = default!;
}