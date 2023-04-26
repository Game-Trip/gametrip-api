using FluentValidation.AspNetCore;
using GameTrip.Domain.Entities;
using GameTrip.Domain.Interfaces;
using GameTrip.Domain.Settings;
using GameTrip.Domain.Tools;
using GameTrip.EFCore;
using GameTrip.EFCore.Data;
using GameTrip.EFCore.Repository;
using GameTrip.EFCore.UnitOfWork;
using GameTrip.Platform;
using GameTrip.Platform.IPlatform;
using GameTrip.Provider;
using GameTrip.Provider.IProvider;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace GameTrip.API;

internal class Startup
{
    #region Properties

    public IConfiguration Configuration { get; }

    #endregion Properties

    #region Constructor

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    #endregion Constructor

    #region Public Methods

    [Obsolete]
    public void ConfigureServices(IServiceCollection services)
    {
        AddServices(services);

        services.AddControllers();
        services.AddFluentValidation();
        AddCORS(services);
        AddJWT(services);
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
                Description = "GameTrip API",
            });
            c.IncludeXmlComments(xmlPath);
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Scheme = "bearer",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                    },
                    new string [] {}
                }
            });
        });

        AddIdentity(services);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, GameTripContext context)
    {
        context.Database.EnsureCreated();

        if (Configuration.GetValue<bool>("UseSwagger"))
        {
            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
                options.EnablePersistAuthorization();
                options.DisplayRequestDuration();
                options.EnableFilter();
                options.EnableTryItOutByDefault();
            });
        }

        ConfigureExceptionHandler(app);

        app.UseRouting();

        app.UseCors();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        ILogger<Program> logger = app.ApplicationServices.GetRequiredService<ILogger<Program>>();
    }

    #endregion Public Methods

    #region Private Methods

    private void AddCORS(IServiceCollection services)
    {
        List<string> originsAllowed = Configuration.GetSection("CallsOrigins").Get<List<string>>()!;
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
                {
                    builder
                           .WithOrigins(originsAllowed.ToArray())
                           .WithMethods("PUT", "DELETE", "GET", "OPTIONS", "POST")
                           .AllowAnyHeader()
                           .Build();
                });
        });
    }

    private void AddJWT(IServiceCollection services)
    {
        JWTSettings? jwtSettings = Configuration.GetSection("JWTSettings").Get<JWTSettings>();
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireAudience = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                ValidateLifetime = true,
                RoleClaimType = "Roles",
                NameClaimType = "User",
            };
        });
        services.AddAuthorization(options =>
        {
            options.DefaultPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddAuthenticationSchemes("Bearer")
                .Build();
        });


        services.Configure<DataProtectionTokenProviderOptions>(o =>
        {
            o.TokenLifespan = TimeSpan.FromHours(1);
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
        services.AddScoped<IAuthPlatform, AuthPlatform>();
        services.AddScoped<IMailPlatform, MailPlatform>();

        #endregion Platform

        #region Provider

        services.AddScoped<IStartupProvider, StartupProvider>();
        services.AddScoped<IEmailProvider, EmailProvider>();

        #endregion Provider

        #region Settings

        //génération du swagger.json from dll buger tkt
        JWTSettings JWTSettings = Configuration.GetSection("JWTSettings").Get<JWTSettings>() ?? new();
        services.AddSingleton(JWTSettings);
        //génération du swagger.json from dll buger tkt
        MailSettings mailSettings = Configuration.GetSection("MailSettings").Get<MailSettings>() ?? new();
        services.AddSingleton(mailSettings);
        //génération du swagger.json from dll buger tkt
        RegisterSettings registerSettings = Configuration.GetSection("RegisterSettings").Get<RegisterSettings>() ?? new();
        services.AddSingleton(registerSettings);
        services.AddScoped<DBInitializer>();

        #endregion Settings
    }

    private void AddIdentity(IServiceCollection services)
    {
        services.AddIdentity<GameTripUser, IdentityRole<Guid>>(options =>
        {
            options.SignIn.RequireConfirmedAccount = true;

            options.ClaimsIdentity.RoleClaimType = ClaimTypes.Role;
            options.ClaimsIdentity.UserIdClaimType = "Id";
            options.ClaimsIdentity.UserNameClaimType = "UserName";
            options.ClaimsIdentity.EmailClaimType = ClaimTypes.Email;

            //Password requirement
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 8;
            options.Password.RequiredUniqueChars = 4; //Determine le nombre de caract�re unnique minimum requis

            //Lockout si mdp fail 5 fois alors compte bloquer 10 min
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(60);
            options.Lockout.AllowedForNewUsers = true;

            //User
            options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            options.User.RequireUniqueEmail = true;

            //Sign
            options.SignIn.RequireConfirmedAccount = true;
        })
        .AddDefaultTokenProviders()
        .AddRoles<IdentityRole<Guid>>()
        .AddEntityFrameworkStores<GameTripContext>();

        services.ConfigureApplicationCookie(options =>
        {
            options.Events.OnRedirectToLogin = context =>
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            };
        });
    }

    private void ConfigureExceptionHandler(IApplicationBuilder app)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                IExceptionHandlerFeature? contextFeatures = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeatures == null) return;

                context.Response.ContentType = "text/html; charset=utf-8";
                string message = string.Empty;
                string user = context?.User?.Identity?.Name ?? "Unknow User";
                if (contextFeatures.Error is ServiceException se)
                {
                    context.Response.StatusCode = (int)se.StatusCode;
                    message = se.Message;
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    message = "Internal Server Error";
                }

                await context.Response.WriteAsync(message);
            });
        });
    }

    #endregion Private Methods
}