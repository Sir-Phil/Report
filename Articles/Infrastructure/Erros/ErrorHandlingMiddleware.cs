using Microsoft.Extensions.Localization;
using System.Net;
using System.Text.Json;

namespace Articles.Infrastructure.Erros
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IStringLocalizer<ErrorHandlingMiddleware> _localizer;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(
            RequestDelegate next, IStringLocalizer<ErrorHandlingMiddleware> localizer, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _localizer = localizer;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, _logger, _localizer);
            }
        }

        private async Task HandleExceptionAsync(
            HttpContext context, 
            Exception exception, 
            ILogger<ErrorHandlingMiddleware> logger, 
            IStringLocalizer<ErrorHandlingMiddleware> localizer)
        {
            string result = null;
            switch (exception)
            {
                case RestException re:
                    context.Response.StatusCode = (int)re.Code;
                    result = JsonSerializer.Serialize(new
                    {
                        errors = re.Errors
                    });
                    break;
                case Exception e:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    logger.LogError(e, "Unhandled Exception");
                    result = JsonSerializer.Serialize(new
                    {
                        errors = localizer[Constants.InternalServerError].Value
                    });
                    break;
            }
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(result ?? "{}");
        }
    }
}
