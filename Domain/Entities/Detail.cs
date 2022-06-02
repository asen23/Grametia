namespace Domain.Entities;

public class Detail : BaseEntity
{
    public Detail()
    {
        Items = new List<DetailItem>();
    }

    public List<DetailItem> Items { get; set; } = default!;

    public long TransactionId { get; set; } = default!;
    public Transaction Transaction { get; set; } = default!;
}