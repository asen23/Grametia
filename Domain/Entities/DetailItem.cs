namespace Domain.Entities;

public class DetailItem : BaseEntity
{
    public int Amount { get; set; } = default!;
    public string BookTitle { get; set; } = default!;
    public long BookPrice { get; set; } = default!;
    public Detail Detail { get; set; } = default!;
}