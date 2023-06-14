using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;
using test_database_first.DBContext;
using test_database_first.Repositories;
namespace test_database_first;
public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true);
        Configuration = builder.Build();
    }

    public void ConfigureServices(IServiceCollection services)
    {
        var connectionString = Configuration.GetValue<string>("ConnectionString");
        services.AddDbContext<PruebaContext>(option => option.UseSqlServer(connectionString));
        services.TryAddScoped(typeof(IRepository<>), typeof(Repository<>));
        var assembly = Assembly.GetExecutingAssembly();
        services.AddMediatR(option => option.RegisterServicesFromAssemblies(assembly));
    }
}

