using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace GameTrip.Domain.Tools;

public class FileUploadFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        IEnumerable<ApiParameterDescription> formParameters = context.ApiDescription.ParameterDescriptions
            .Where(paramDesc => paramDesc.IsFromForm());

        if (formParameters.Any())
        {
            // already taken care by swashbuckle. no need to add explicitly.
            return;
        }
        if (operation.RequestBody != null)
        {
            // NOT required for form type
            return;
        }
        if (context.ApiDescription.HttpMethod == HttpMethod.Post.Method)
        {
            OpenApiMediaType uploadFileMediaType = new()
            {
                Schema = new OpenApiSchema()
                {
                    Type = "object",
                    Properties =
                    {
                        ["files"] = new OpenApiSchema()
                        {
                            Type = "array",
                            Items = new OpenApiSchema()
                            {
                                Type = "string",
                                Format = "binary"
                            }
                        }
                    },
                    Required = new HashSet<string>() { "files" }
                }
            };

            operation.RequestBody = new OpenApiRequestBody
            {
                Content = { ["multipart/form-data"] = uploadFileMediaType }
            };
        }
    }
}

public static class Helper
{
    internal static bool IsFromForm(this ApiParameterDescription apiParameter)
    {
        BindingSource source = apiParameter.Source;
        Type? elementType = apiParameter.ModelMetadata?.ElementType;

        return source == BindingSource.Form || source == BindingSource.FormFile
            || (elementType != null && typeof(IFormFile).IsAssignableFrom(elementType));
    }
}