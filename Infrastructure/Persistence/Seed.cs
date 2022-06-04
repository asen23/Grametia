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

        context.Books.Add(new Book
        {
            Title = "The Adventures of Sherlock Holmes",
            Edition = "Reissue",
            Description =
                "The Adventures of Sherlock Holmes is a collection of twelve short stories by Arthur Conan Doyle, first published on 14 October 1892. It contains the earliest short stories featuring the consulting detective Sherlock Holmes, which had been published in twelve monthly issues of The Strand Magazine from July 1891 to June 1892. The stories are collected in the same sequence, which is not supported by any fictional chronology. The only characters common to all twelve are Holmes and Dr. Watson and all are related in first-person narrative from Watson's point of view.",
            Author = "Arthur Conan Doyle",
            Publisher = "Harper & Brothers",
            ISBN = "0199536953",
            Category = "Mystery",
            ReleaseDate = "1949",
            Price = 200000,
            Stock = 100,
        });

        context.Books.Add(new Book
        {
            Title = "1984",
            Edition = "1",
            Description = "It was a bright cold day in April, and the clocks were striking thirteen.",
            Author = "George Orwell",
            Publisher = "Amazing Reads",
            ISBN = "9788192910901",
            Category = "Thriller",
            ReleaseDate = "1892",
            Price = 250000,
            Stock = 0,
        });

        context.Books.Add(new Book
        {
            Title = "The Hitch Hiker's Guide to the Galaxy",
            Edition = "Paperback",
            Description = "The Hitchhiker's Guide to the Galaxy is the first of six books in the Hitchhiker's Guide to the Galaxy comedy science fiction \"hexalogy\" by Douglas Adams. The novel is an adaptation of the first four parts of Adams's radio series of the same name. The novel was first published in London on 12 October 1979. It sold 250,000 copies in the first three months. ",
            Author = "Douglas Adams",
            Publisher = "Book Club Associates",
            ISBN = "58600649",
            Category = "Science Fiction",
            ReleaseDate = "1979",
            Price = 150000,
            Stock = 5,
        });

        await context.SaveChangesAsync(cancellationToken);
    }
}