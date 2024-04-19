using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OtpServer.Exception;

namespace OtpServer.Controllers.Filter
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is ApiException apiEx)
            {
                HandleApiException(apiEx, context);
            }
            else
            {
                context.Result = new StatusCodeResult(500);
                context.ExceptionHandled = true;
            }
        }

        private void HandleApiException(ApiException apiEx, ExceptionContext context)
        {
            var response = new { message = apiEx.Message, code = apiEx.ErrorCode };
            context.Result = apiEx.StatusCode switch
            {
                ErrorStatusCodes.BadRequest => new BadRequestObjectResult(response),
                ErrorStatusCodes.NotFound => new NotFoundObjectResult(response),
                ErrorStatusCodes.Unauthorized => new UnauthorizedResult(),
                _ => new StatusCodeResult(500)
            };
            context.ExceptionHandled = true;
        }
    }
}
