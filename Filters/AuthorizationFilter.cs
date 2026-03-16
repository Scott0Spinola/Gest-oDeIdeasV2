using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GestãoDeIdeasV2.Filters
{
    /// <summary>
    /// Authorization filter to validate API requests based on API key.
    /// Checks for a valid API key in the 'X-API-Key' header.
    /// </summary>
    public class ApiKeyAuthorizationFilter : IAsyncAuthorizationFilter
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ApiKeyAuthorizationFilter> _logger;

        public ApiKeyAuthorizationFilter(IConfiguration configuration, ILogger<ApiKeyAuthorizationFilter> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            _logger.LogInformation("Authorization filter executing for {Controller}.{Action}",
                context.RouteData.Values["controller"],
                context.RouteData.Values["action"]);

            // Skip authorization for Swagger endpoints
            if (context.HttpContext.Request.Path.StartsWithSegments("/swagger") ||
                context.HttpContext.Request.Path.StartsWithSegments("/swagger-ui"))
            {
                return;
            }

            // Check if the action or controller has [AllowAnonymous] attribute
            if (context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any())
            {
                return;
            }

            // API Key is required for authorization
            var validApiKey = _configuration.GetValue<string>("ApiKey");
            _logger.LogInformation("Configured API Key: '{ApiKey}' (Length: {Length})", validApiKey, validApiKey?.Length ?? 0);
            var isAuthorized = false;

            if (context.HttpContext.Request.Headers.TryGetValue("X-API-Key", out var headerApiKey))
            {
                var trimmedHeaderKey = headerApiKey.ToString().Trim();
                _logger.LogInformation("API Key from header: '{HeaderApiKey}' (Length: {Length})", trimmedHeaderKey, trimmedHeaderKey.Length);
                if (trimmedHeaderKey.Equals(validApiKey, StringComparison.OrdinalIgnoreCase))
                {
                    isAuthorized = true;
                }
            }
            else if (context.HttpContext.Request.Query.TryGetValue("apiKey", out var queryApiKey))
            {
                var trimmedQueryKey = queryApiKey.ToString().Trim();
                _logger.LogInformation("API Key from query: '{QueryApiKey}' (Length: {Length})", trimmedQueryKey, trimmedQueryKey.Length);
                if (trimmedQueryKey.Equals(validApiKey, StringComparison.OrdinalIgnoreCase))
                {
                    isAuthorized = true;
                }
            }
            else
            {
                _logger.LogWarning("No API Key found in headers or query parameters");
            }

            if (!isAuthorized)
            {
                _logger.LogWarning("Authorization failed - missing or invalid API Key from {RemoteIpAddress}",
                    context.HttpContext.Connection.RemoteIpAddress);
                context.Result = new UnauthorizedObjectResult(new { message = "Missing or Invalid API Key" });
                return;
            }

            _logger.LogInformation("Request authorized for {RemoteIpAddress}",
                context.HttpContext.Connection.RemoteIpAddress);

            await Task.CompletedTask;
        }
    }
}
