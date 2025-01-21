using Bobs_Corn_Challenge.API.Exceptions;
using Bobs_Corn_Challenge.Entities;

namespace Bobs_Corn_Challenge.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var response = new ErrorResponse();

            switch (exception)
            {
                case OtherException otherEx:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    response.Message = otherEx.Message;
                    response.ErrorCode = "OTHER_ERROR";
                    break;

                case RateLimitExceededException rateLimitEx:
                    context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    response.Message = rateLimitEx.Message;
                    response.ErrorCode = "RATE_LIMIT_EXCEEDED";
                    break;

                default:
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    response.Message = "An unexpected error occurred. Please try again later.";
                    response.ErrorCode = "INTERNAL_ERROR";
                    break;
            }

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
