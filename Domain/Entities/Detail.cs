namespace Domain.Entities;

public class Detail : BaseEntity
{
    public User User { get; set; } = default!;

    public List<DetailItem> Items { get; set; } = default!;
}