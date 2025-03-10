using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace UrlShortener.API.Extensions;

public class GlobalValidationFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .Select(x => new
                {
                    Field = x.Key,
                    Errors = x.Value.Errors.Select(e => e.ErrorMessage)
                })
                .ToList();

            context.Result = new BadRequestObjectResult(new
            {
                Message = "Invalid input data",
                Errors = errors
            });
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    { }
}
