using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;


namespace EngMonarchApi
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        
        public ErrorHandlingMiddleware(
         RequestDelegate next,
         ILogger<ErrorHandlingMiddleware> logger)
        {
            this._next = next;
            this._logger = logger;
        }
               
        public async Task Invoke(HttpContext context /* other scoped dependencies */)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                await HandleExceptionAsync(context, ex, _logger);
            }
        }

        private static async Task HandleExceptionAsync(
            HttpContext context,
            Exception exception,
            ILogger<ErrorHandlingMiddleware> logger)
        {
            var code = HttpStatusCode.InternalServerError; // 500 if unexpected

            //if (exception is NotFoundException) code = HttpStatusCode.NotFound;
            //else if (exception is UnauthorizedException) code = HttpStatusCode.Unauthorized;
            //else if (exception is CustomException) code = HttpStatusCode.BadRequest;
            object errors = null;
            switch (exception)
            {
                case RestException re:
                    errors = re.Errors;
                    code = (HttpStatusCode) re.Code;
                    break;
                case Exception e:
                    errors = string.IsNullOrWhiteSpace(e.Message) ? "Error" : e.Message;
                    break;
            }

            // Using RFC 7807 response for error formatting
            // https://tools.ietf.org/html/rfc7807
            var problem = new ProblemDetails
            {
                Type = "https://yourdomain.com/errors/internal-server-error",
                Title = exception.Message,
                Detail = errors?.ToString() ?? exception.Message,
                Instance = "",
                Status = (int)code
            };

            var result = JsonConvert.SerializeObject(
                problem,
                new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = (int)code;

            await context.Response.WriteAsync(result);
        }
    }

    public class RestException : Exception
    {
        public RestException(HttpStatusCode code, object errors = null)
        {
            Code = code;
            Errors = errors;
        }

        public object Errors { get; set; }

        public HttpStatusCode Code { get; }
    }
}
