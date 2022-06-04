#region

using Application.Common.Interfaces;
using Grametia.Menu;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

#endregion

var app = Host.CreateDefaultBuilder()
    .ConfigureServices((_, services) =>
        services
            .AddApplicationServices()
            .AddInfrastructureServices()
    )
    .Build();

RunApp(app.Services);
await app.StartAsync();

static async void RunApp(IServiceProvider services)
{
    var scope = services.CreateScope();
    var provider = scope.ServiceProvider;

    var db = provider.GetRequiredService<ApplicationDbContext>();
    await db.Database.MigrateAsync();

    Seed.Initialize(provider.GetRequiredService<IApplicationDbContext>(), CancellationToken.None);

    var mediator = provider.GetRequiredService<ISender>();

    try
    {
        await new MainMenu(mediator).Run();
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
    }
}