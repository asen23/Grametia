#region

using Application.Users.Commands.CreateUser;
using Application.Users.Commands.LoginCommand;
using MediatR;

#endregion

namespace Grametia.Menu;

public class MainMenu : IMenu
{
    private readonly ISender _mediator;

    public MainMenu(ISender mediator)
    {
        _mediator = mediator;
    }

    public async Task Run()
    {
        while (true)
        {
            Input.Clear();
            Input.WriteHeader("Welcome to Grametia");
            Input.WriteLine("1. Login");
            Input.WriteLine("2. Register");
            Input.WriteLine("3. Guest");
            Input.WriteLine("0. Exit");
            var choice = Input.ReadInt(">> ");
            switch (choice)
            {
                case 1:
                    await Login();
                    break;
                case 2:
                    await Register();
                    break;
                case 3:
                    await new GuestMenu(_mediator).Run();
                    break;
                case 0:
                    return;
            }
        }
    }

    private async Task Login()
    {
        while (true)
        {
            Input.Clear();
            Input.WriteHeader("Login");
            var email = Input.ReadLine("Email : ");
            var password = Input.ReadLine("Password : ");
            var result = await _mediator.Send(new LoginCommand
            {
                Email = email,
                Password = password,
            });
            if (!result.IsValidResponse)
            {
                Input.WriteLine(result.ErrorMessage);
                if(Input.TryAgain()) continue;
                Input.Prompt();
                return;
            }

            if (result.Result == "member")
            {
                await new MemberMenu(_mediator).Run();
                return;
            }

            await new AdminMenu(_mediator).Run();
            return;
        }
    }

    private async Task Register()
    {
        while (true)
        {
            Input.Clear();
            Input.WriteHeader("Register");
            var username = Input.ReadLine("Username : ");
            var email = Input.ReadLine("Email : ");
            var password = Input.ReadLine("Password : ");
            var address = Input.ReadLine("Address : ");
            var phoneNumber = Input.ReadLine("Phone Number : ");
            var result = await _mediator.Send(new CreateUserCommand
            {
                Username = username,
                Email = email,
                Password = password,
                Address = address,
                PhoneNumber = phoneNumber,
                Role = "member",
            });
            if (result.IsValidResponse)
            {
                Input.WriteLine("Successfully registered! Please login with your new credential");
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