using MicroFinance.Exceptions;

namespace MicroFinance.ServiceExtensions.ApplicationService
{
    public static class ApplicationMiddlewareExtension
    {
        public static IApplicationBuilder AddGlobalErrorHandler(this IApplicationBuilder applicationBuilder)
        => applicationBuilder.UseMiddleware<GlobalExceptionHandlingMiddleware>();
    }
}