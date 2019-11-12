using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Moz.Exceptions
{
    public interface IExceptionHandler
    {
        Task HandleExceptionAsync(RequestDelegate next, HttpContext context, Exception exception);
    }
}