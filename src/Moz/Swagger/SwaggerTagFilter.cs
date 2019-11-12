using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Moz.Swagger
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class SwaggerTagFilter : IDocumentFilter
    {
        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            foreach(var contextApiDescription in context.ApiDescriptions)
            {
                var actionDescriptor = (ControllerActionDescriptor)contextApiDescription.ActionDescriptor;

                if(!actionDescriptor.ControllerTypeInfo.GetCustomAttributes<SwaggerTagAttribute>().Any() && 
                   !actionDescriptor.MethodInfo.GetCustomAttributes<SwaggerTagAttribute>().Any())
                {
                    var key = "/" + contextApiDescription.RelativePath.TrimEnd('/');
                    swaggerDoc.Paths.Remove(key);
                }
            }
        }
    }
}