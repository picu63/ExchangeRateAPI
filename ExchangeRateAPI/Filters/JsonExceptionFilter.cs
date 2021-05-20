using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Exceptions;
using Microsoft.OpenApi.Models;

namespace ExchangeRateAPI.Filters
{
    public class JsonExceptionFilter : IExceptionFilter
    {
        private readonly IWebHostEnvironment _environment;

        public JsonExceptionFilter(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public void OnException(ExceptionContext context)
        {
            if (!_environment.IsDevelopment())
            {
                context.Result = new ObjectResult(new {context.Exception.Message, context.Exception.StackTrace});
            }
        }
    }
}
