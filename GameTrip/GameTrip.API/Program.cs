using Microsoft.AspNetCore;

namespace GameTrip.API;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    private static IWebHostBuilder CreateHostBuilder(string[] args)
    {
        IConfigurationRoot? config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();

        return WebHost.CreateDefaultBuilder(args).UseConfiguration(config)
                .UseStartup<Startup>();
    }
}