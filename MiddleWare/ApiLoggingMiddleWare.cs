using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MicroFinance.Middleware
{
    public class ApiLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiLoggingMiddleware> _logger;

        public ApiLoggingMiddleware(RequestDelegate next, ILogger<ApiLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            // Capture the request
            var request = context.Request;
            var requestBody = await FormatRequest(request);

            _logger.LogInformation($"{DateTime.Now}: API Got hit");
            _logger.LogInformation($"Request: {request.Method} {request.Path}{request.QueryString}");
            _logger.LogInformation($"Request Body: {requestBody}");

            if (context.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
            {
                var token = authorizationHeader.ToString().Replace("Bearer ", "");
                var decodedToken = DecodeJwtToken(token);
                _logger.LogInformation($"JWT Token Claims: {FormatClaims(decodedToken.Claims)}");
            }

            using var responseBody = new MemoryStream();
            var originalBodyStream = context.Response.Body;
            context.Response.Body = responseBody;

            await _next(context);

            var response = context.Response;
            var responseBodyString = await FormatResponse(response);

            // Log response information
            _logger.LogInformation($"Response: {response.StatusCode}");
            if(response.StatusCode>=400)
                _logger.LogInformation($"Response Body: {responseBodyString}");
            responseBody.Seek(0, SeekOrigin.Begin);
            await responseBody.CopyToAsync(originalBodyStream);
        }

        private async Task<string> FormatRequest(HttpRequest request)
        {
            var body = request.Body;

            request.EnableBuffering();

            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            var requestBody = Encoding.UTF8.GetString(buffer);
            request.Body.Seek(0, SeekOrigin.Begin);

            return $"{requestBody}";
        }

        private async Task<string> FormatResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            return $"{responseBody}";
        }

        private static JwtSecurityToken DecodeJwtToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            return handler.ReadJwtToken(token);
        }

        private static string FormatClaims(IEnumerable<Claim> claims)
        {
            return string.Join(", ", claims.Select(claim => $"{claim.Type}: {claim.Value}"));
        }

    }
}
