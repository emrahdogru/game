using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace War.Server
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var response = new
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Message = context.Exception.Message,
                Type = context.Exception.GetType().Name
            };

            context.Result = new JsonResult(response)
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };

            context.ExceptionHandled = true; // Hatanın işlendiğini belirtir
        }
    }
}
