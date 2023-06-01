using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GameTrip.EFCore.IOC;
public static class ServiceCollectionExtension
{
    public static void AddGameTripDbContext(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddDbContext<GameTripContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("GameTripSQL"),
                x => x.MigrationsAssembly(typeof(GameTripContext).Assembly.FullName)));
    }

    public static void ConfigureDatabase(this IServiceProvider services)
    {
        using (IServiceScope serviceScope = services.CreateScope())
        {
            GameTripContext? context = serviceScope.ServiceProvider.GetService<GameTripContext>();
            if (context != null)
            {
                if (context.Database.IsRelational())
                    context?.Database.Migrate();
                else
                {
                    context.Database.EnsureCreated();
                }
            }
        }
    }
}
