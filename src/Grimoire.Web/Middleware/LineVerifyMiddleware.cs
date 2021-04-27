using Grimoire.Web.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Grimoire.Web.Middleware
{
    public class LineVerifyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly LineSignatureService _validator;

        public LineVerifyMiddleware(RequestDelegate next, LineSignatureService validator)
        {
            _next = next;
            _validator = validator;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            httpContext.Request.EnableBuffering();
            var request = httpContext.Request;
            
            using var streamReader = new StreamReader(request.Body);
            { 
                var txt = await streamReader.ReadToEndAsync();
                Console.WriteLine(txt);
                request.Body.Seek(0, SeekOrigin.Begin);

                var signature = Convert.FromBase64String(request.Headers["x-line-signature"]);
                bool res = await _validator.ValidateSignatureAsync(request.Body, signature);
                request.Body.Seek(0, SeekOrigin.Begin);
            
                await _next(httpContext);
            };
        }
    }
}
