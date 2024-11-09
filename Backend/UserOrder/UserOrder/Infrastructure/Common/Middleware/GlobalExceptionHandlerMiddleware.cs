using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using UserOrder.Application.Responses;

namespace UserOrder.Infrastructure.Common.Middleware
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;
        public GlobalExceptionHandlerMiddleware(RequestDelegate next,ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex,ex.Message);
                await HandleValidationExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
                _logger.LogError(ex,ex.Message);
            }
        }
        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var response = new ApiResponse<object>()
            {
                Message = ex.Message,
                StatusCode = -2
            };

            return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }

        private static Task HandleValidationExceptionAsync(HttpContext context, ValidationException ex)
        {
            var response = new ApiResponse<object>()
            {
                Message = ex.Message,
                StatusCode = -1
            };

            return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }
}