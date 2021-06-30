using LinkFinder.WebApi.Logic.Errors;
using LinkFinder.WebApi.Logic.Response.Models.Errors;
using Microsoft.AspNetCore.Http;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace LinkFinder.WebApi.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JsonSerializerOptions jsonSerializerOptions;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;

            jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
        }

        public async Task Invoke(HttpContext context)
        {

            try
            {
                await _next(context);
            }
            catch (InvalidInputUrlException e)
            {
                context.Response.StatusCode = 400;
                context.Response.ContentType = "application/json";

                var errorModel = new ErrorResponse("InvalidInputUrl", e.Message);

                var serialized = JsonSerializer.Serialize(errorModel, jsonSerializerOptions);

                await context.Response.WriteAsync(serialized);
            }
            catch (InvalidOperationException e)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 400;

                var errorModel = new ErrorResponse("InvalidInputUrl", e.Message);

                if (e.Message == "Sequence contains no elements")
                {
                    errorModel.ErrorMessage = "Not found request item";
                }

                var serialized = JsonSerializer.Serialize(errorModel, jsonSerializerOptions);
                await context.Response.WriteAsync(serialized);
            }
            catch (Exception e)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 500;

                var errorModel = new ErrorResponse("InvalidInputUrl", e.Message);

                var serialized = JsonSerializer.Serialize(errorModel, jsonSerializerOptions);

                await context.Response.WriteAsync(serialized);
            }

        }
    }
}
