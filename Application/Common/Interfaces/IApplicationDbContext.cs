#region

using Domain.Entities;
using Microsoft.EntityFrameworkCore;

#endregion

namespace Application.Common.Interfaces;

public interface IApplicationDbContext
{
    public DbSet<Book> Books { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}