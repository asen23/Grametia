#region

using Application.Books.Commands.CreateBook;
using Application.Books.Commands.DeleteBook;
using Application.Books.Commands.UpdateBook;
using Application.Transactions.Queries;
using Application.Users.Commands.CreateUser;
using Application.Users.Commands.DeleteUser;
using Application.Users.Commands.LogoutCommand;
using Application.Users.Queries;
using MediatR;

#endregion

namespace Grametia.Menu;

public class AdminMenu : GuestMenu
{
    public AdminMenu(ISender mediator) : base(mediator)
    {
    }

    public new async Task Run()
    {
        while (true)
        {
            Input.Clear();
            Input.WriteHeader("Admin");
            Input.WriteLine("1. Add Book");
            Input.WriteLine("2. Update Book");
            Input.WriteLine("3. Delete Book");
            Input.WriteLine("4. View Books");
            Input.WriteLine("5. View Book Detail");
            Input.WriteLine("6. Add User");
            Input.WriteLine("7. Delete User");
            Input.WriteLine("8. View Users");
            Input.WriteLine("9. View User Detail");
            Input.WriteLine("10. View Transactions");
            Input.WriteLine("11. View Transaction Detail");
            Input.WriteLine("0. Logout");
            var choice = Input.ReadInt(">> ");
            switch (choice)
            {
                case 1:
                    await AddBook();
                    break;
                case 2:
                    await UpdateBook();
                    break;
                case 3:
                    await DeleteBook();
                    break;
                case 4:
                    await ViewBooks();
                    break;
                case 5:
                    await ViewBookDetail();
                    break;
                case 6:
                    await AddUser();
                    break;
                case 7:
                    await DeleteUser();
                    break;
                case 8:
                    await ViewUsers();
                    break;
                case 9:
                    await ViewUserDetail();
                    break;
                case 10:
                    await ViewTransactions();
                    break;
                case 11:
                    await ViewTransactionDetail();
                    break;
                case 0:
                    await Mediator.Send(new LogoutCommand());
                    return;
            }
        }
    }

    private async Task AddBook()
    {
        while (true)
        {
            Input.Clear();
            Input.WriteHeader("Add Book");
            var title = Input.ReadLine("Title : ");
            var edition = Input.ReadLine("Edition : ");
            var description = Input.ReadLine("Description : ");
            var author = Input.ReadLine("Author : ");
            var publisher = Input.ReadLine("Publisher : ");
            var isbn = Input.ReadLine("ISBN : ");
            var category = Input.ReadLine("Category : ");
            var releaseDate = Input.ReadLine("Release Date : ");
            var price = Input.ReadLong("Price : ");
            var stock = Input.ReadInt("Stock : ");
            var result = await Mediator.Send(new CreateBookCommand
            {
                Title = title,
                Edition = edition,
                Description = description,
                Author = author,
                Publisher = publisher,
                ISBN = isbn,
                Category = category,
                ReleaseDate = releaseDate,
                Price = price,
                Stock = stock,
            });
            if (result.IsValidResponse)
            {
                Input.WriteLine("Successfully added book");
                Input.Prompt();
                return;
            }

            Input.WriteLine(result.ErrorMessage);
            Input.Prompt();
        }
    }

    private async Task UpdateBook()
    {
        while (true)
        {
            Input.Clear();
            Input.WriteHeader("Update Book");

            var booksNotEmpty = await PrintBooks();
            if (!booksNotEmpty)
            {
                Input.WriteLine("No books exist");
                Input.Prompt();
                return;
            }

            var id = Input.ReadLong("Id : ");
            Input.WriteLine("Press enter to keep existing value");
            var title = Input.ReadLine("Title : ");
            var edition = Input.ReadLine("Edition : ");
            var description = Input.ReadLine("Description : ");
            var author = Input.ReadLine("Author : ");
            var publisher = Input.ReadLine("Publisher : ");
            var isbn = Input.ReadLine("ISBN : ");
            var category = Input.ReadLine("Category : ");
            var releaseDate = Input.ReadLine("Release Date : ");
            Input.WriteLine("Input -1 to keep existing value");
            var price = Input.ReadLong("Price : ");
            var stock = Input.ReadInt("Stock : ");
            var result = await Mediator.Send(new UpdateBookCommand
            {
                Id = id,
                Title = title,
                Edition = edition,
                Description = description,
                Author = author,
                Publisher = publisher,
                ISBN = isbn,
                Category = category,
                ReleaseDate = releaseDate,
                Price = price,
                Stock = stock,
            });

            if (result.IsValidResponse)
            {
                Input.WriteLine("Successfully updated book");
                Input.Prompt();
                return;
            }

            Input.WriteLine(result.ErrorMessage);
            Input.Prompt();
        }
    }

    private async Task DeleteBook()
    {
        while (true)
        {
            Input.Clear();
            Input.WriteHeader("Delete Book");
            var booksNotEmpty = await PrintBooks();
            if (!booksNotEmpty)
            {
                Input.WriteLine("No books exist");
                Input.Prompt();
                return;
            }

            Input.WriteLine("Input -1 to cancel");
            var id = Input.ReadLong("Id : ");
            if (id == -1) return;
            var result = await Mediator.Send(new DeleteBookCommand(id));
            if (result.IsValidResponse)
            {
                Input.WriteLine("Successfully deleted book");
                Input.Prompt();
                return;
            }

            Input.WriteLine(result.ErrorMessage);
            Input.Prompt();
        }
    }

    private async Task AddUser()
    {
        while (true)
        {
            Input.Clear();
            Input.WriteHeader("Add User");
            var username = Input.ReadLine("Username : ");
            var email = Input.ReadLine("Email : ");
            var password = Input.ReadLine("Password : ");
            var address = Input.ReadLine("Address : ");
            var phoneNumber = Input.ReadLine("Phone Number : ");
            var role = Input.ReadLine("Role (member or admin) : ");
            var result = await Mediator.Send(new CreateUserCommand
            {
                Username = username,
                Email = email,
                Password = password,
                Address = address,
                PhoneNumber = phoneNumber,
                Role = role,
            });
            if (result.IsValidResponse)
            {
                Input.WriteLine("Successfully added user");
                Input.Prompt();
                return;
            }

            Input.WriteLine(result.ErrorMessage);
            if(Input.TryAgain()) continue;
            Input.Prompt();
            return;
        }
    }

    private async Task<bool> PrintUsers()
    {
        var users = await Mediator.Send(new GetUsers());
        var header = $"| {"Id",-5} | {"Username",-20} | {"Email",-20} | {"Role",20} |";
        Input.WriteSeparator(header.Length);
        Input.WriteLine(header);
        Input.WriteSeparator(header.Length);
        foreach (var user in users)
            Input.WriteLine($"| {user.Id,-5} | {user.Username,-20} | {user.Email,-20} | {user.Role,20} |");
        Input.WriteSeparator(header.Length);
        return users.Any();
    }

    private async Task ViewUsers()
    {
        Input.Clear();
        Input.WriteHeader("User List");
        await PrintUsers();
        Input.WriteLine();
        Input.Prompt();
    }

    private async Task ViewUserDetail()
    {
        Input.Clear();
        Input.WriteHeader("User Detail");

        var booksNotEmpty = await PrintUsers();
        if (!booksNotEmpty)
        {
            Input.WriteLine("No books exist");
            Input.Prompt();
            return;
        }

        var id = Input.ReadLong("Id : ");
        var result = await Mediator.Send(new GetUserById(id));

        if (!result.IsValidResponse)
        {
            Input.WriteLine(result.ErrorMessage);
            Input.Prompt();
            return;
        }

        var user = result.Result;
        Input.Clear();
        Input.WriteHeader(user.Username);
        Input.WriteLine($"Id            : {user.Id}");
        Input.WriteLine($"Email         : {user.Email}");
        Input.WriteLine($"Address       : {user.Address}");
        Input.WriteLine($"Phone Number  : {user.PhoneNumber}");
        Input.WriteLine($"Role          : {user.Role}");
        Input.Prompt();
    }

    private async Task DeleteUser()
    {
        while (true)
        {
            Input.Clear();
            Input.WriteHeader("Delete User");
            var usersNotEmpty = await PrintUsers();
            if (!usersNotEmpty)
            {
                Input.WriteLine("No users exist");
                Input.Prompt();
                return;
            }

            Input.WriteLine("Input -1 to cancel");
            var id = Input.ReadLong("Id : ");
            if (id == -1) return;
            var result = await Mediator.Send(new DeleteUserCommand(id));
            if (result.IsValidResponse)
            {
                Input.WriteLine("Successfully deleted user");
                Input.Prompt();
                return;
            }

            Input.WriteLine(result.ErrorMessage);
            if(Input.TryAgain()) continue;
            Input.Prompt();
            return;
        }
    }

    protected override async Task<bool> PrintTransactions()
    {
        var transactions = await Mediator.Send(new GetTransactions());
        var header = $"| {"Id",-5} | {"Username",-20} | {"Date",-20} | {"Total",20} |";
        Input.WriteSeparator(header.Length);
        Input.WriteLine(header);
        Input.WriteSeparator(header.Length);
        foreach (var transaction in transactions)
            Input.WriteLine(
                $"| {transaction.Id,-5} | {transaction.User.Username,-20} | {transaction.DateTime,-20} | {transaction.Detail.Items.Select(di => di.Amount * di.Book.Price).Sum(),20} |");
        Input.WriteSeparator(header.Length);

        return transactions.Any();
    }
}