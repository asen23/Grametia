namespace Domain.Entities;

public class Book : BaseEntity
{
    public string Title { get; set; } = default!;
    public string Edition { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string Author { get; set; } = default!;
    public string Publisher { get; set; } = default!;
    public string ISBN { get; set; } = default!;
    public string Category { get; set; } = default!;
    public string ReleaseDate { get; set; } = default!;
    public long Price { get; set; } = default!;
    public int Stock { get; set; } = default!;
}