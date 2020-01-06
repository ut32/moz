using Moz.Bus.Dtos;

namespace Moz.Bus.Services
{
    public class BaseService
    {
        protected ServOkResult Ok()
        {
            return new ServOkResult();
        }

        protected ServErrorResult Error(string msg = "发生错误")
        {
            return new ServErrorResult(msg); 
        }
    }
}