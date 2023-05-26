using System.Net;
using System.Text.Json;

namespace MicroFinance.Exceptions
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private class ErrorResponse
        {
            public string? Message { get; set; }
            public string? StatusTrace { get; set; }
            public HttpStatusCode? Status { get; set; }
        }

        private static ErrorResponse GetErrorResponse(string message, string statusTrace, HttpStatusCode status)
        {
            return new ErrorResponse
            {
                Message = message,
                Status = status,
                StatusTrace = statusTrace
            };
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            ErrorResponse errorResponse = new ErrorResponse();

            switch (ex)
            {
                case NotFoundExceptionHandler exception:
                    errorResponse =
                    GetErrorResponse(exception.Message + exception.InnerException?.Message, 
                    exception.StackTrace, HttpStatusCode.NotFound);
                    break;

                case BadRequestExceptionHandler exception:
                    errorResponse =
                    GetErrorResponse(exception.Message + exception.InnerException?.Message, 
                    exception.StackTrace, HttpStatusCode.BadRequest);
                    break;

                case KeyNotFoundExceptionHandler exception:
                    errorResponse =
                    GetErrorResponse(exception.Message + exception.InnerException?.Message, 
                    exception.StackTrace, HttpStatusCode.NotAcceptable);
                    break;

                case NotImplementedExceptionHandler exception:
                    errorResponse =
                    GetErrorResponse(exception.Message + exception.InnerException?.Message, 
                    exception.StackTrace, HttpStatusCode.NotImplemented);
                    break;
                
                case UnAuthorizedExceptionHandler exception:
                    errorResponse = 
                    GetErrorResponse(exception.Message + exception.InnerException?.Message, 
                    exception.StackTrace, HttpStatusCode.Unauthorized);
                    break;

                default:
                    errorResponse = 
                    GetErrorResponse(ex.Message + ex.InnerException?.Message, 
                    ex.StackTrace, HttpStatusCode.Unauthorized);
                    break;
            }
            
            var errors = new Dictionary<string, List<string>>
            {
                {"Message", new List<string>{errorResponse.Message}},
                {"StatusTrace", new List<string>{errorResponse.StatusTrace}}
            };

            var exceptionResult = JsonSerializer.Serialize(new {errors});
            //JsonSerializer.Serialize(new {error = errorResponse.Message, errorResponse.StatusTrace});
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) errorResponse.Status;
            return context.Response.WriteAsync(exceptionResult);
        }
    }
}