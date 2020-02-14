using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace Moz.Exceptions
{
    public interface IStatusCodePageHandler
    {
        Task Process(StatusCodeContext context);
    }
}