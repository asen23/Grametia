#region

using Application.Common.Interfaces;
using Domain.Entities;

#endregion

namespace Infrastructure.Persistence;

public class Seed
{
    public static async void Initialize(IApplicationDbContext context, CancellationToken cancellationToken)
    {
        if (context.Users.Any()) return;

        context.Users.Add(new User
        {
            Id = 1,
            Username = "Admin",
            Address = "Admin Street",
            Email = "admin@admin.com",
            Password = "root",
            PhoneNumber = "08123456789",
            Role = "admin",
        });

        await context.SaveChangesAsync(cancellationToken);
    }
}