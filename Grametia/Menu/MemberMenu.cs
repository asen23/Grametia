#region

using Application.Cart.Command.ProcessCart;
using Application.CartItems.Commands.AddCartItem;
using Application.CartItems.Commands.UpdateCartItem;
using Application.CartItems.Queries;
using Application.Users.Commands.LogoutCommand;
using Domain.Entities;
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
            if(Input.TryAgain()) continue;
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
            Input.WriteLine($"| {cartItem.Book.Id,-5} | {cartItem.Book.Title,-50} | {cartItem.Book.Price,-20} | {cartItem.Amount,20} |");
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
            if(Input.TryAgain()) continue;
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
            if(confirm != "y") return;

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
            if(Input.TryAgain()) continue;
            Input.Prompt();
            return;
        }
    }
}