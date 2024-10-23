using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace MBS.Application.Exceptions
{
    public class ExceptionHandlingMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            var response = context.Response;

            var errorResponse = new ErrorResponse { TimeStamp = DateTime.UtcNow, Error = ex.Message };

            switch (ex)
            {
                case BadHttpRequestException:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse.StatusCode = (int)HttpStatusCode.BadRequest;
                    _logger.LogInformation(ex.ToString());
                    break;

                case UnauthorizedAccessException:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    errorResponse.StatusCode = (int)HttpStatusCode.Unauthorized;
                    _logger.LogInformation(ex.ToString());
                    break;

                case NotFoundException:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    errorResponse.StatusCode = (int)HttpStatusCode.NotFound;
                    _logger.LogInformation(ex.ToString());
                    break;

                case ForbiddenAccessException:
                    response.StatusCode = (int)HttpStatusCode.Forbidden;
                    errorResponse.StatusCode = (int)HttpStatusCode.Forbidden;
                    _logger.LogInformation(ex.ToString());
                    break;

                // JWT-specific exceptions
                case SecurityTokenExpiredException: // Token hết hạn
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    errorResponse.StatusCode = (int)HttpStatusCode.Unauthorized;
                    errorResponse.Error = "Token has expired.";
                    _logger.LogInformation(ex.ToString());
                    break;

                case SecurityTokenInvalidSignatureException: // Chữ ký token không hợp lệ
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    errorResponse.StatusCode = (int)HttpStatusCode.Unauthorized;
                    errorResponse.Error = "Invalid token signature.";
                    _logger.LogInformation(ex.ToString());
                    break;

                case SecurityTokenInvalidIssuerException: // Issuer không hợp lệ
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    errorResponse.StatusCode = (int)HttpStatusCode.Unauthorized;
                    errorResponse.Error = "Invalid token issuer.";
                    _logger.LogInformation(ex.ToString());
                    break;

                case SecurityTokenInvalidAudienceException: // Audience không hợp lệ
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    errorResponse.StatusCode = (int)HttpStatusCode.Unauthorized;
                    errorResponse.Error = "Invalid token audience.";
                    _logger.LogInformation(ex.ToString());
                    break;

                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorResponse.StatusCode = (int)HttpStatusCode.InternalServerError;
                    _logger.LogInformation(ex.ToString());
                    break;
            }

            var result = JsonConvert.SerializeObject(errorResponse);
            await context.Response.WriteAsync(result);
        }

        public class ErrorResponse
        {
            public int StatusCode { get; set; }

            public string Error { get; set; }

            public DateTime TimeStamp { get; set; }

            //public override string ToString()
            //{
            //    return JsonSerializer.Serialize(this);
            //}
        }
    }
}