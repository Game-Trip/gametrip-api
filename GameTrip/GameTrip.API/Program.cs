using FluentValidation.AspNetCore;
using GameTrip.API.Extension;
using GameTrip.EFCore.IOC;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddFluentValidation();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddGameTripDbContext(builder.Configuration);

builder.Services.AddCORS(builder.Configuration);
builder.Services.AddJWT(builder.Configuration);
builder.Services.ConfigureIdentity();
builder.Services.AddSwagger();

builder.Services.AddServices(builder.Configuration);
builder.Services.AddValidators();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.Services.ConfigureDatabase();
}

if (builder.Configuration.GetValue<bool>("UseSwagger"))
    app.ConfigureSwagger();

app.ConfigureExceptionHandler();

app.UseRouting();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints => endpoints.MapControllers());

app.Run();

public partial class Program { }
