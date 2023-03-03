using FluentValidation.AspNetCore;
using GameTrip.Domain.Interfaces;
using GameTrip.EFCore;
using GameTrip.EFCore.Repository;
using GameTrip.EFCore.UnitOfWork;
using GameTrip.Platform;
using GameTrip.Platform.IPlatform;
using GameTrip.Provider;
using GameTrip.Provider.IProvider;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace GameTrip.API;

internal class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        AddServices(services);

        services.AddControllers();
        services.AddFluentValidation();

        AddDatabase(services);

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            string? API_NAME = Assembly.GetExecutingAssembly().GetName().Name;
            string xmlPath = $"{AppContext.BaseDirectory}{API_NAME}.xml";

            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = API_NAME,
                Description = "Fifty Cent API",
            });
            //c.IncludeXmlComments(xmlPath);
        });
    }

    private void AddDatabase(IServiceCollection services)
    {
        services.AddDbContext<GameTripContext>(options => options.UseSqlServer(Configuration.GetConnectionString("GameTripSQL"),
                                                                            x => x.MigrationsAssembly(typeof(GameTripContext).Assembly.FullName)));
    }

    private void AddServices(IServiceCollection services)
    {
        #region UnitOfWork

        services.AddTransient<IUnitOfWork, UnitOfWork>();

        #endregion UnitOfWork

        #region Repository

        services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddTransient<ICommentRepository, CommentRepository>();
        services.AddTransient<IGameRepository, GameRepository>();
        services.AddTransient<ILikedGameRepository, LikedGameRepository>();
        services.AddTransient<ILikedLocationRepository, LikedLocationRepository>();
        services.AddTransient<ILocationRepository, LocationRepository>();
        services.AddTransient<IPictureRepository, PictureRepository>();

        #endregion Repository

        #region Platform

        services.AddScoped<IStartupPlatform, StartupPlatform>();

        #endregion Platform

        #region Provider

        services.AddScoped<IStartupProvider, StartupProvider>();

        #endregion Provider
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        using (var serviceScope = app.ApplicationServices.CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetService<GameTripContext>();
            context.Database.EnsureCreated();
        }

        if (env.IsDevelopment())
        {
            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.InjectStylesheet("/swagger-ui/SwaggerDark.css"); //Get Swagger in dark mode
                options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
            });
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        var logger = app.ApplicationServices.GetRequiredService<ILogger<Program>>();
    }
}