using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateAPI.Data;
using ExchangeRateAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ExchangeRateAPI
{
    public class RestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public RestLoggingMiddleware(RequestDelegate next, ILogger<RestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Writes the request to the database as intended.
        /// </summary>
        private async Task SaveContextOperation(HttpContext httpContext, ExchangeRateAPIContext exchangeRateApiContext)
        {
            try
            {
                var requestItem = await GetRequestItem(httpContext.Request);
                var responseItem = await GetResponseItem(httpContext.Response);
                await exchangeRateApiContext.RequestResponseItems.AddAsync(new RequestResponseItem()
                { Request = requestItem, Response = responseItem });
                await exchangeRateApiContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error with saving request-response");
                throw;
            }
        }

        private async Task<ResponseItem> GetResponseItem(HttpResponse httpResponse)
        {
            _logger.LogInformation($"Creating {nameof(ResponseItem)} item...");
            var responseItem = new ResponseItem()
            {
                CreationTime = DateTime.Now, 
                StatusCode = httpResponse.StatusCode
            };
            var headers = string.Empty;
            foreach (var requestHeader in httpResponse.Headers)
            {
                headers += requestHeader + Environment.NewLine;
            }
            responseItem.Headers = headers;
            return await Task.FromResult(responseItem);
        }

        private async Task<RequestItem> GetRequestItem(HttpRequest httpRequest)
        {
            _logger.LogInformation($"Creating {nameof(RequestItem)} item...");
            var requestItem = new RequestItem {CreationTime = DateTime.Now};
            httpRequest.EnableBuffering();
            httpRequest.Body.Position = 0;
            StreamReader sr = new StreamReader(httpRequest.Body);
            requestItem.Body = await sr.ReadToEndAsync();
            var headers = string.Empty;
            foreach (var requestHeader in httpRequest.Headers)
            {
                headers += requestHeader + Environment.NewLine;
            }

            requestItem.Url = httpRequest.GetDisplayUrl();
            requestItem.Method = httpRequest.Method;
            requestItem.Headers = headers;
            return requestItem;
        }

        public async Task InvokeAsync(HttpContext context, ExchangeRateAPIContext dbContext)
        {
            try
            {
                await _next(context);
            }
            finally
            {
                await SaveContextOperation(context, dbContext);
            }
        }
    }
}
