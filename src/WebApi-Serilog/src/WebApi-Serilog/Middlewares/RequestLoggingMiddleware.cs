using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi_Serilog
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var requestBody = await FormatRequest(context.Request);
            if (!string.IsNullOrEmpty(requestBody))
                _logger.LogInformation("Request Body {body}", requestBody);

            await _next(context);

            //using (var stream = new MemoryStream())
            //{
            //    var originalBodyStream = context.Response.Body;
            //    context.Response.Body = stream;

            //    await _next(context);

            //    var reponseBody = await FormatResponse(context.Response);
            //    if (!string.IsNullOrEmpty(reponseBody))
            //        _logger.LogInformation("Response Body {reponseBody}", reponseBody);

            //    context.Response.Body = originalBodyStream;
            //}
        }

        private async Task<string> FormatRequest(HttpRequest request)
        {
            request.EnableBuffering();
            request.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(request.Body).ReadToEndAsync();
            request.Body.Seek(0, SeekOrigin.Begin);
            return text;
        }

        private async Task<string> FormatResponse(HttpResponse response)
        {
            if (response.HasStarted)
                return string.Empty;

            response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            return text;
        }
    }
}
