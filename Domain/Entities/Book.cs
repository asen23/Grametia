namespace Domain.Entities;

public class Book : BaseEntity
{
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string Author { get; set; } = default!;
    public long Price { get; set; } = default!;
    public int Stock { get; set; } = default!;
}