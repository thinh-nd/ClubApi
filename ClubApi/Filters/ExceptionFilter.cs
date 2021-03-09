using ClubApi.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ClubApi.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is ClientException exception)
            {
                context.Result = new ObjectResult(new
                {
                    Error = exception.Message
                })
                {
                    StatusCode = (int)exception.StatusCode
                };
            }
            else
            {
                context.Result = new ObjectResult(new
                {
                    Error = "Internal error occurred."
                })
                {
                    StatusCode = 500
                };
            }
        }
    }
}
