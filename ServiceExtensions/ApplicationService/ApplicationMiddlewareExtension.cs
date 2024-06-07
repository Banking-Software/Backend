using MicroFinance.Exceptions;
using MicroFinance.Middleware;

namespace MicroFinance.ServiceExtensions.ApplicationService
{
    public static class ApplicationMiddlewareExtension
    {
        public static IApplicationBuilder AddGlobalErrorHandler(this IApplicationBuilder applicationBuilder)
        => applicationBuilder.UseMiddleware<ApiLoggingMiddleware>().UseMiddleware<GlobalExceptionHandlingMiddleware>();
    }

    
}