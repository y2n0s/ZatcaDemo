using Application.Exceptions;
using Application.Responses;
using System.Net;
using System.Text.Json;

namespace Presentation.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionHandlerMiddleware(RequestDelegate next,
            ILogger<ExceptionHandlerMiddleware> logger,
            IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception e)
        {
            context.Response.ContentType = "application/json";
            HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;

            var response = string.Empty;

            var serializeOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            switch (e)
            {
                case BadRequestException badRequestException:
                    httpStatusCode = HttpStatusCode.BadRequest;
                    response = JsonSerializer.Serialize(new ErrorResponse((int)httpStatusCode,
                        badRequestException.Message), serializeOptions);
                    break;
              
                case Exception exception:
                    httpStatusCode = HttpStatusCode.InternalServerError;
                    var result = _env.IsDevelopment() ?
                    new ExceptionResponse(StatusCodes.Status500InternalServerError,
                    e.Message, e.StackTrace.ToString()) :
                    new ExceptionResponse(StatusCodes.Status500InternalServerError);
                    response = JsonSerializer.Serialize(result, serializeOptions);
                    break;
            }

            context.Response.StatusCode = (int)httpStatusCode;
            context.Response.Redirect("/Home/error");
            //await context.Response.WriteAsync(response);
        }
    }
}
