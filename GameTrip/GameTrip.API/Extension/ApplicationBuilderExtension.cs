using GameTrip.Domain.Tools;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace GameTrip.API.Extension;

public static class ApplicationBuilderExtension
{
    public static void ConfigureSwagger(this IApplicationBuilder app)
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
    public static void ConfigureExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                IExceptionHandlerFeature? contextFeatures = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeatures == null)
                    return;

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
}
