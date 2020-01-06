using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Moz.Exceptions
{
    public interface IExceptionHandler
    {
        Task<IsExceptionHandled> HandleExceptionAsync(HttpContext context, Exception exception);
    }
} 