namespace eCommerce.Products.API.Middleware;

public class ExceptionMiddlewareHandler
{
    private readonly ILogger<ExceptionMiddlewareHandler> _logger;
    private readonly RequestDelegate _next;
    
    public ExceptionMiddlewareHandler(ILogger<ExceptionMiddlewareHandler> logger, RequestDelegate next)
    {
        _logger = logger;
        _next  = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            _logger.LogError($"{e.GetType()}: {e.Message}");

            if (e.InnerException != null)
            {
                _logger.LogError($"{e.InnerException.GetType()}: {e.InnerException.Message}");
            }

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(new { Message = e.Message, Type = e.GetType().ToString() });
        }
    }
}

public static class ExceptionMiddlewareHandlerExtensions
{
    public static IApplicationBuilder UseExceptionMiddlewareHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionMiddlewareHandler>();
    }
}