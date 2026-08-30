using System.Net;
using System.Text.Json;

namespace Test.Presentation.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";

                context.Response.StatusCode = ex switch
                {
                    // 400 Bad Request: Invalid input data
                    ArgumentException => (int)HttpStatusCode.BadRequest,

                    // 404 Not Found: Resource doesn't exist
                    KeyNotFoundException => (int)HttpStatusCode.NotFound,

                    // 409 Conflict: Business rule violation (e.g., double booking)
                    InvalidOperationException => (int)HttpStatusCode.Conflict,

                    // 500 Internal Server Error: Unhandled crash
                    _ => (int)HttpStatusCode.InternalServerError
                };

                var result = JsonSerializer.Serialize(new { error = ex.Message });
                await context.Response.WriteAsync(result);
            }
        }
    }
}
