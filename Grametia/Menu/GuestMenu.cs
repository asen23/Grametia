#region

using Application.Books.Queries;
using MediatR;

#endregion

namespace Grametia.Menu;

public class GuestMenu : IMenu
{
    protected readonly ISender Mediator;

    public GuestMenu(ISender mediator)
    {
        Mediator = mediator;
    }

    public async Task Run()
    {
        while (true)
        {
            Input.Clear();
            Input.WriteHeader("Guest");
            Input.WriteLine("1. View Books");
            Input.WriteLine("2. View Book Detail");
            Input.WriteLine("0. Exit");
            var choice = Input.ReadInt(">> ");
            switch (choice)
            {
                case 1:
                    await ViewBooks();
                    break;
                case 2:
                    await ViewBookDetail();
                    break;
                case 0:
                    return;
            }
        }
    }

    protected async Task ViewBooks()
    {
        Input.Clear();
        Input.WriteHeader("Book List");
        await PrintBooks();
        Input.WriteLine();
        Input.Prompt();
    }

    protected async Task<bool> PrintBooks()
    {
        var books = await Mediator.Send(new GetBooks());
        var header = $"| {"Id",-5} | {"Title",-50} | {"Stock",-10} | {"Price",20} |";
        Input.WriteSeparator(header.Length);
        Input.WriteLine(header);
        Input.WriteSeparator(header.Length);
        foreach (var book in books)
        {
            if (book.Stock == 0) Console.ForegroundColor = ConsoleColor.Red;
            Input.WriteLine($"| {book.Id,-5} | {book.Title,-50} | {book.Stock,-10} | {book.Price,20} |");
            Console.ResetColor();
        }
        Input.WriteSeparator(header.Length);
        return books.Any();
    }

    protected async Task ViewBookDetail()
    {
        Input.Clear();
        Input.WriteHeader("Book Detail");

        var booksNotEmpty = await PrintBooks();
        if (!booksNotEmpty)
        {
            Input.WriteLine("No books exist");
            Input.Prompt();
            return;
        }

        var id = Input.ReadLong("Id : ");

        var result = await Mediator.Send(new GetBookById(id));
        if (!result.IsValidResponse)
        {
            Input.WriteLine(result.ErrorMessage);
            Input.Prompt();
            return;
        }

        var book = result.Result;
        Input.Clear();
        Input.WriteHeader(book.Title);

        Input.WriteLine($"Id            : {book.Id}");
        Input.WriteLine($"Edition       : {book.Edition}");
        Input.WriteLine($"ISBN          : {book.ISBN}");
        Input.WriteLine($"Author        : {book.Author}");
        Input.WriteLine($"Publisher     : {book.Publisher}");
        Input.WriteLine($"Category      : {book.Category}");
        Input.WriteLine($"ReleaseDate   : {book.ReleaseDate}");
        Input.WriteLine($"Price         : {book.Price}");
        Input.WriteLine($"Stock         : {book.Stock}");

        Input.WriteHeader("Description");

        var words = book.Description.Split(' ');
        var lines = words.Skip(1).Aggregate(words.Take(1).ToList(), (l, w) =>
        {
            if (l.Last().Length + w.Length >= 80)
                l.Add(w);
            else
                l[^1] += " " + w;
            return l;
        });

        Input.WriteLine(string.Join('\n', lines));
        Input.Prompt();
    }
}