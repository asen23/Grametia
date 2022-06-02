#region

using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

#endregion

namespace Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = Path.Join(path, "database.db");
    }

    private string DbPath { get; }
    public DbSet<Book> Books { get; set; } = default!;
    public DbSet<User> Users { get; set; } = default!;
    public DbSet<Transaction> Transactions { get; set; } = default!;

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite($"Data Source={DbPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasOne(u => u.Cart)
            .WithOne(c => c.User)
            .HasForeignKey<Cart>(c => c.UserId);
        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.Detail)
            .WithOne(d => d.Transaction)
            .HasForeignKey<Detail>(d => d.TransactionId);
        modelBuilder.Entity<User>()
            .HasData(new User
            {
                Id = 1,
                Username = "Admin",
                Address = "Admin Street",
                Email = "admin@admin.com",
                Password = "root",
                PhoneNumber = "08123456789",
                Role = "admin"
            });
    }
}