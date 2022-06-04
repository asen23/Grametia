#region

using Application.Cart.Command.ProcessCart;
using Application.CartItems.Commands.AddCartItem;
using Application.CartItems.Commands.UpdateCartItem;
using Application.CartItems.Queries;
using Application.Transactions.Queries;
using Application.Users.Commands.LogoutCommand;
using MediatR;

#endregion

namespace Grametia.Menu;

public class MemberMenu : GuestMenu
{
    public MemberMenu(ISender mediator) : base(mediator)
    {
    }

    public new async Task Run()
    {
        while (true)
        {
            Input.Clear();
            Input.WriteHeader("Member");
            Input.WriteLine("1. View Books");
            Input.WriteLine("2. View Book Detail");
            Input.WriteLine("3. Add Book To Cart");
            Input.WriteLine("4. Update Cart");
            Input.WriteLine("5. View Cart");
            Input.WriteLine("6. Process Cart");
            Input.WriteLine("7. View Transactions");
            Input.WriteLine("8. View Transaction Detail");
            Input.WriteLine("0. Logout");
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
                    await AddBookToCart();
                    break;
                case 4:
                    await UpdateCart();
                    break;
                case 5:
                    await ViewCartItems();
                    break;
                case 6:
                    await ProcessCart();
                    break;
                case 7:
                    await ViewTransactions();
                    break;
                case 8:
                    await ViewTransactionDetail();
                    break;
                case 0:
                    await Mediator.Send(new LogoutCommand());
                    return;
            }
        }
    }

    private async Task AddBookToCart()
    {
        while (true)
        {
            Input.Clear();
            Input.WriteHeader("Add Book To Cart");

            var booksNotEmpty = await PrintBooks();
            if (!booksNotEmpty)
            {
                Input.WriteLine("No books exist");
                Input.Prompt();
                return;
            }

            var id = Input.ReadLong("BookId : ");
            var amount = Input.ReadInt("Amount : ");

            var result = await Mediator.Send(new AddCartItemCommand
            {
                BookId = id,
                Amount = amount,
            });
            if (result.IsValidResponse)
            {
                Input.WriteLine("Successfully added book to cart");
                Input.Prompt();
                return;
            }

            Input.WriteLine(result.ErrorMessage);
            if (Input.TryAgain()) continue;
            Input.Prompt();
            return;
        }
    }

    private async Task ViewCartItems()
    {
        Input.Clear();
        Input.WriteHeader("Cart List");
        await PrintCartItems();
        Input.WriteLine();
        Input.Prompt();
    }

    private async Task<bool> PrintCartItems()
    {
        var cartItems = await Mediator.Send(new GetCartItemByUserId());
        var header = $"| {"BookId",-5} | {"Title",-50} | {"Price",-20} | {"Amount",20} |";
        Input.WriteSeparator(header.Length);
        Input.WriteLine(header);
        Input.WriteSeparator(header.Length);
        foreach (var cartItem in cartItems)
            Input.WriteLine(
                $"| {cartItem.Book.Id,-5} | {cartItem.Book.Title,-50} | {cartItem.Book.Price,-20} | {cartItem.Amount,20} |");
        Input.WriteSeparator(header.Length);
        return cartItems.Any();
    }

    private async Task UpdateCart()
    {
        while (true)
        {
            Input.Clear();
            Input.WriteHeader("Update Cart");

            var booksNotEmpty = await PrintCartItems();
            if (!booksNotEmpty)
            {
                Input.WriteLine("No item exist in cart");
                Input.Prompt();
                return;
            }

            var id = Input.ReadLong("BookId : ");
            Input.WriteLine("Input 0 to remove or -1 to cancel");
            var amount = Input.ReadInt("Amount : ");

            if (amount == -1) return;

            var result = await Mediator.Send(new UpdateCartItemCommand
            {
                BookId = id,
                Amount = amount,
            });
            if (result.IsValidResponse)
            {
                Input.WriteLine("Successfully updated cart");
                Input.Prompt();
                return;
            }

            Input.WriteLine(result.ErrorMessage);
            if (Input.TryAgain()) continue;
            Input.Prompt();
            return;
        }
    }

    private async Task ProcessCart()
    {
        while (true)
        {
            Input.Clear();
            Input.WriteHeader("Process Cart");

            var confirm = Input.ReadLine("Are you sure? (y/n) : ");
            if (confirm != "y") return;

            var courier = Input.ReadLine("Courier : ");
            var paymentMethod = Input.ReadLine("Payment Method : ");

            var result = await Mediator.Send(new ProcessCartCommand
            {
                Courier = courier,
                PaymentMethod = paymentMethod,
            });
            if (result.IsValidResponse)
            {
                Input.WriteLine("Successfully processed your cart");
                Input.Prompt();
                return;
            }

            Input.WriteLine(result.ErrorMessage);
            if (Input.TryAgain()) continue;
            Input.Prompt();
            return;
        }
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