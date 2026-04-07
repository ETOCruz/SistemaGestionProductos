using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Tools;

namespace WebApi.Middleware
{
    public class ExceptionLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _logPath;

        public ExceptionLoggingMiddleware(RequestDelegate next, string logPath)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logPath = logPath ?? throw new ArgumentNullException(nameof(logPath));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var traceId = Activity.Current?.Id ?? context.TraceIdentifier;

                try
                {
                    var log = Log.GetInstance(_logPath);
                    var message = $"traceId={traceId} [{DateTime.UtcNow:O}] {context.Request.Method} {context.Request.Path} - {ex}";
                    log.Save(message);
                }
                catch
                { }

                var env = context.RequestServices.GetService(typeof(IHostEnvironment)) as IHostEnvironment;
                var problem = new ProblemDetails
                {
                    Title = "Algo salio mal :(",
                    Status = StatusCodes.Status500InternalServerError,
                    Detail = env?.IsDevelopment() == true ? ex.ToString() : "Se produjo un error inesperado.",
                    Instance = context.Request.Path
                };
                problem.Extensions["traceId"] = traceId;

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/problem+json";

                try
                {
                    await context.Response.WriteAsJsonAsync(problem);
                }
                catch
                {
                    try
                    {
                        await context.Response.WriteAsync("Internal Server Error");
                    }
                    catch { }
                }
            }
        }
    }
}
