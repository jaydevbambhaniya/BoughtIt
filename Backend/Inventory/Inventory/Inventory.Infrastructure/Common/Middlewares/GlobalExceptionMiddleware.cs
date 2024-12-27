using Application.Common.Responses;
using Domain.Common;
using Domain.Common.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
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
            catch (CustomException ex)
            {
                _logger.LogError(ex,ex.Message);
                await HandleCustomException(context,ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                await HandleException(context, ex);
            }
        }
        private Task HandleCustomException(HttpContext context,CustomException ex)
        {
            var response = new ApiResponse<object>()
            {
                StatusCode = ex.Code,
                Message = ex.Message
            };
            return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
        private Task HandleException(HttpContext context, Exception ex)
        {
            var error = ErrorCodes.GetError("SERVER_ERROR");
            var response = new ApiResponse<object>()
            {
                StatusCode = error.Code,
                Message = error.Message
            };
            return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }
}
