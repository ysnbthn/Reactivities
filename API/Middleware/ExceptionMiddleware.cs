using System.Net;
using System.Text.Json;
using Application.Core;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        public RequestDelegate _next;
        public ILogger<ExceptionMiddleware> _logger;
        public IWebHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context){
            try
            {
                // eğer sorun yoksa sıradaki middlewareden devam et
                await _next(context);
            }
            catch (Exception ex)
            {
               _logger.LogError(ex, ex.Message);
               context.Response.ContentType = "application/json";
               context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
               // development modundaysa stacktrace ile hatayı ver yoksa sadece kod ile mesaj döndür 
               var response = _env.IsDevelopment() 
                    ? new AppException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
                    : new AppException(context.Response.StatusCode, "Server Error");
                // yazı tipini camel case yap sonra jsona dönüştür
                var options = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
                var json = JsonSerializer.Serialize(response, options);
                // response olarak json objesini yap
                await context.Response.WriteAsync(json);
            }
        }
    }
}