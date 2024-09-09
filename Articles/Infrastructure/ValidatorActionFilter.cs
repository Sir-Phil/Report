using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

namespace Articles.Infrastructure
{
    public class ValidatorActionFilter : IActionFilter 
    {
        private readonly ILogger _logger;

        public ValidatorActionFilter(ILogger<ValidatorActionFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (!filterContext.ModelState.IsValid)
            {
                var result = new ContentResult();
                var error = new Dictionary<string, string[]>();

                foreach (var valuePair in filterContext.ModelState)
                {
                    error.Add(valuePair.Key, valuePair.Value.Errors.Select(x => x.ErrorMessage).ToArray());
                }

                string content = JsonSerializer.Serialize(new { error });
                result.Content = content;
                result.ContentType = "application/json";

                filterContext.HttpContext.Response.StatusCode = 422;//uprocessed entity
                filterContext.Result = result;
            }
        }    
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
         
        }
    }
}
