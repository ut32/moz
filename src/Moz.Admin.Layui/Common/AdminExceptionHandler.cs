using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moz.Bus.Dtos;
using Moz.Exceptions;
using Newtonsoft.Json;

namespace Moz.Admin.Layui.Common
{
    public class AdminExceptionHandler : AbstractExceptionHandler<AdminExceptionHandler>
    {
        public AdminExceptionHandler(ILogger<AdminExceptionHandler> logger, IWebHostEnvironment webHostEnvironment) 
            : base(logger, webHostEnvironment)
        {
        }
    }
}