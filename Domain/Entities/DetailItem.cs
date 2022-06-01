namespace Domain.Entities;

public class DetailItem : BaseEntity
{
    public int Amount { get; set; } = default!;
    public Book Book { get; set; } = default!;
    public Detail Detail { get; set; } = default!;
}