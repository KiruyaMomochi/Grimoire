using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Grimoire.LineApi;
using Grimoire.Web.Services;
using Microsoft.AspNetCore.Http;

namespace Grimoire.Web.Middleware
{
    public class LineEndpoint
    {
        private readonly RequestDelegate _next;
        private readonly LineSignatureService _validator;
        private readonly CommandManager _manager;
        private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);

        public LineEndpoint(RequestDelegate next, LineSignatureService validator, CommandManager manager)
        {
            _next = next;
            _validator = validator;
            _manager = manager;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            httpContext.Request.EnableBuffering();
            var request = httpContext.Request.Body;
            
            if (!httpContext.Request.Headers.TryGetValue("x-line-signature", out var signatureString))
            {
                httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                await httpContext.Response.WriteAsync("No signature provided");
                return;
            }
            
            var signature = Convert.FromBase64String(signatureString);
            var res = await _validator.ValidateSignatureAsync(request, signature);
            if (!res)
            {
                httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                await httpContext.Response.WriteAsync("Invalid signature");
                return;
            }
            
            request.Seek(0, SeekOrigin.Begin);
            var we = await JsonSerializer.DeserializeAsync<WebhookEvent>(request, SerializerOptions);

            Debug.Assert(we != null, nameof(we) + " != null");
            foreach (var e in we.Events)
                await _manager.HandleWebhookEvent(e);
            await httpContext.Response.WriteAsync("success");
        }
    }
}