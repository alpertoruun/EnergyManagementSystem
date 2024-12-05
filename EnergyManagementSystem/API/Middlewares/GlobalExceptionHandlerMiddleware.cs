using System.Net;
using System.Text.Json;

namespace EnergyManagementSystem.API.Middleware
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                var errorResponse = new ErrorResponse
                {
                    Success = false,
                    Message = error.Message
                };

                switch (error)
                {
                    case KeyNotFoundException:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;

                    case UnauthorizedAccessException:
                        response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        break;

                    case ArgumentException:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;

                    case FluentValidation.ValidationException:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;

                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        errorResponse.Message = "Internal Server Error";
                        _logger.LogError(error, error.Message);
                        break;
                }

                await response.WriteAsync(JsonSerializer.Serialize(errorResponse));
            }
        }
    }

    public class ErrorResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public static class GlobalExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            return app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
        }
    }
}