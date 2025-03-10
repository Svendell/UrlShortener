using UrlShortener.Shared.Exceptions;

namespace UrlShortener.API.Extensions
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
            catch (BusinessException ex)
            {
                _logger.LogWarning(ex, "Business exception occurred: {Message}", ex.Message);
                await HandleBusinessExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred: {Message}", ex.Message);
                await HandleGenericExceptionAsync(context, ex);
            }
        }

        private static async Task HandleBusinessExceptionAsync(HttpContext context, BusinessException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";

            var errorDetails = new
            {
                ex.ErrorCode,
                Message = ex.UserMessage
            };

            if (context.Request.Headers["Accept"].ToString().Contains("text/html"))
            {
                context.Response.Redirect($"/Home/Error?errorCode={ex.ErrorCode}&message={Uri.EscapeDataString(ex.UserMessage)}");
            }
            else
            {
                await context.Response.WriteAsJsonAsync(errorDetails);
            }
        }

        private static async Task HandleGenericExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            var errorDetails = new
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An unexpected error occurred. Please try again later."
            };

            if (context.Request.Headers["Accept"].ToString().Contains("text/html"))
            {
                context.Response.Redirect($"/Home/Error?errorCode={errorDetails.ErrorCode}&message={Uri.EscapeDataString(errorDetails.Message)}");
            }
            else
            {
                await context.Response.WriteAsJsonAsync(errorDetails);
            }
        }
    }

    public static class ExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
