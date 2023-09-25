using CA.Application.Exceptions;
using Newtonsoft.Json;
using System.Net;

namespace CA.Api.Middleware
{

    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next)
        {
            int num = 0;
            var x = num.ToString().Select(p => new { x = (int)p }).ToList().Sum(p => p.x);
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }
        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            string result = JsonConvert.SerializeObject(new ErrorDeatils
            {
                ErrorMessage = exception.Message,
                ErrorType = "Failure"
            });

            switch (exception)
            {
                case BadRequestException badRequestException:
                    statusCode = HttpStatusCode.BadRequest;
                    result = JsonConvert.SerializeObject(badRequestException.Message);
                    break;
                case CustomValidationException validationException:
                    statusCode = HttpStatusCode.BadRequest;
                    result = JsonConvert.SerializeObject(validationException.Errors);
                    break;
                case NotFoundException notFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    result = JsonConvert.SerializeObject(exception.Message);
                    break;
                case Microsoft.Data.SqlClient.SqlException:
                    result = JsonConvert.SerializeObject(exception.Message);
                    statusCode = HttpStatusCode.Conflict;
                    break;
                case Microsoft.EntityFrameworkCore.DbUpdateException:
                    result = exception.InnerException.Message;
                    statusCode = HttpStatusCode.Conflict;
                    break;
                default:
                    result = exception.Message;
                    statusCode = HttpStatusCode.Conflict;
                    break;
            }

            context.Response.StatusCode = (int)statusCode;
            return context.Response.WriteAsync(result);
        }
    }

    public class ErrorDeatils
    {
        public string ErrorType { get; set; }
        public string ErrorMessage { get; set; }
    }
}
