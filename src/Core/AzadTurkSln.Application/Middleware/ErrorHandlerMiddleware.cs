using AzadTurkSln.Application.Exceptions;
using AzadTurkSln.Application.Wrappers;
using Microsoft.AspNetCore.Http;
using Serilog;
using System.Net;
using System.Text.Json;

namespace AzadTurkSln.Application.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                await HandleExceptionAsync(context, error);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception error)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            var responseModel = new ServiceResponse<string>() { IsSuccess = false, Message = error.Message };

            // Create structured log properties
            var logProperties = new Dictionary<string, object>
            {
                ["UserName"] = context.User?.Identity?.Name ?? "Anonymous",
                ["ClientIp"] = context.Connection.RemoteIpAddress?.ToString(),
                ["RequestPath"] = context.Request.Path,
                ["ExceptionType"] = error.GetType().Name,
                ["StackTrace"] = error.StackTrace
            };

            switch (error)
            {
                case ValidationException:
                    response.StatusCode = (int) HttpStatusCode.BadRequest;
                    Log.Warning(error, "Validation Exception occurred {@LogProperties}", logProperties);
                    break;
                case NotFoundException:
                    response.StatusCode = (int) HttpStatusCode.NotFound;
                    Log.Warning(error, "Not Found Exception occurred {@LogProperties}", logProperties);
                    break;
                case ApiException:
                    response.StatusCode = (int) HttpStatusCode.BadRequest;
                    Log.Warning(error, "API Exception occurred {@LogProperties}", logProperties);
                    break;
                default:
                    response.StatusCode = (int) HttpStatusCode.InternalServerError;
                    Log.Error(error, "Unhandled Exception occurred {@LogProperties}", logProperties);
                    break;
            }

            var result = JsonSerializer.Serialize(responseModel);
            await response.WriteAsync(result);

        }
    }
}