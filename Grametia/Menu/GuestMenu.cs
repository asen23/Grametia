#region

using Application.Books.Queries;
using Application.Transactions.Queries;
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
            Input.WriteLine("3. View Transactions");
            Input.WriteLine("4. View Transaction Detail");
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
                case 3:
                    await ViewTransactions();
                    break;
                case 4:
                    await ViewTransactionDetail();
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
        var header = $"| {"Id",-5} | {"Title",-50} | {"Author",-20} | {"Price",20} |";
        Input.WriteSeparator(header.Length);
        Input.WriteLine(header);
        Input.WriteSeparator(header.Length);
        foreach (var book in books)
            Input.WriteLine($"| {book.Id,-5} | {book.Title,-50} | {book.Author,-20} | {book.Price,20} |");
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

    protected virtual async Task<bool> PrintTransactions()
    {
        var transactions = await Mediator.Send(new GetTransactionsByUserId());
        var header = $"| {"Id",-5} | {"Username",-20} | {"Date",-20} | {"Total",20} |";
        Input.WriteSeparator(header.Length);
        Input.WriteLine(header);
        Input.WriteSeparator(header.Length);
        foreach (var transaction in transactions)
            Input.WriteLine(
                $"| {transaction.Id,-5} | {transaction.User.Username,-20} | {transaction.DateTime,-20} | {transaction.Detail.Items.Select(di => di.Amount * di.BookPrice).Sum(),20} |");
        Input.WriteSeparator(header.Length);

        return transactions.Any();
    }

    protected async Task ViewTransactions()
    {
        Input.Clear();
        Input.WriteHeader("Transaction List");
        await PrintTransactions();
        Input.WriteLine();
        Input.Prompt();
    }

    protected async Task ViewTransactionDetail()
    {
        Input.Clear();
        Input.WriteHeader("Transaction Detail");

        var booksNotEmpty = await PrintTransactions();
        if (!booksNotEmpty)
        {
            Input.WriteLine("No books exist");
            Input.Prompt();
            return;
        }

        var id = Input.ReadLong("Id : ");
        
        var result = await Mediator.Send(new GetTransactionById(id));
        if (!result.IsValidResponse)
        {
            Input.WriteLine(result.ErrorMessage);
            Input.Prompt();
            return;
        }

        var transaction = result.Result;
        Input.Clear();
        Input.WriteHeader("Transaction");
        Input.WriteLine($"Id            : {transaction.Id}");
        Input.WriteLine($"Username      : {transaction.User.Username}");
        Input.WriteLine($"Date          : {transaction.DateTime}");
        Input.WriteLine($"Total         : {transaction.Detail.Items.Select(di => di.Amount * di.BookPrice).Sum()}");
        Input.WriteHeader("Detail");
        foreach (var (detailItem, i) in transaction.Detail.Items.Select((i, idx) => (i, idx)))
            Input.WriteLine($"{i + 1}. {detailItem.BookTitle} x{detailItem.Amount}, {detailItem.BookPrice} each");
        Input.Prompt();
    }
}