using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using test_database_first.Services.Ventas;

namespace test_database_first;
public class Program
{
    static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        var mediator = host.Services.GetRequiredService<IMediator>();
        var result = await mediator.Send(new GetInfo());
        foreach (var item in result)
        {
            Console.Out.WriteLine(item);
        }
        Console.ReadLine();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostBuilderContext, serviceCollection) => new Startup(hostBuilderContext.Configuration).ConfigureServices(serviceCollection));
}

