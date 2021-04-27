using System;
using System.Threading.Tasks;
using Grimoire.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Grimoire.Web.Filters
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ValidateLineWebhookAttribute : ActionFilterAttribute
    {
        public ValidateLineWebhookAttribute()
        {
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var request = context.HttpContext.Request;
            var signature = Convert.FromBase64String(request.Headers["x-line-signature"]);
            // if (!_validator.ValidateSignature(request.Body, signature)) context.Result = new ForbidResult();

            base.OnActionExecuting(context);
        }
    }

    public class ValidateLineWebhookServiceFilter : IAsyncActionFilter
    {
        private readonly LineSignatureService _validator;

        public ValidateLineWebhookServiceFilter(LineSignatureService validator)
        {
            _validator = validator;
        }
        
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var request = context.HttpContext.Request;
            var signature = Convert.FromBase64String(request.Headers["x-line-signature"]);
            if (await _validator.ValidateSignatureAsync(request.Body, signature))
                await next();
            else
                context.Result = new BadRequestObjectResult(context.ModelState);
        }
    }
}