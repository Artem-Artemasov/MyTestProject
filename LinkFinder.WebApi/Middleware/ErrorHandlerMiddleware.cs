using LinkFinder.WebApi.Logic.Exceptions;
using LinkFinder.WebApi.Logic.Response.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace LinkFinder.WebApi.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JsonSerializerOptions jsonSerializerOptions;
        private readonly IWebHostEnvironment _environment;

        public ErrorHandlerMiddleware(RequestDelegate next, IWebHostEnvironment env)
        {
            _next = next;

            jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                IgnoreNullValues = true,
            };

            _environment = env;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (InvalidInputUrlException e)
            {
                await AddExceptionToResponse(400, e, context);
            }
            catch (ObjectNotFoundException e)
            {
                await AddExceptionToResponse(404, e, context);
            }
            catch (Exception e)
            {
                await AddExceptionToResponse(500, e, context);
            }
        }

        private async Task AddExceptionToResponse(int statusCode, Exception exception, HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var errorModel = new ResponseObject()
            {
                IsSuccessful = false,
                Errors = exception.ToString() + exception.Message,
            };

            if (_environment.IsDevelopment())
            {
                errorModel = await AddEnviromentInfo(exception, errorModel);
            }

            var serialized = JsonSerializer.Serialize(errorModel, jsonSerializerOptions);

            await context.Response.WriteAsync(serialized);


        }

        private async Task<ResponseObject> AddEnviromentInfo(Exception exception, ResponseObject responseObject)
        {
            responseObject.Errors += exception.StackTrace;

            return responseObject;
        }
    }
}
