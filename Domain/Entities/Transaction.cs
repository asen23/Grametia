namespace Domain.Entities;

public class Transaction : BaseEntity
{
    public DateTime DateTime { get; set; } = default!;

    public User User { get; set; } = default!;
}