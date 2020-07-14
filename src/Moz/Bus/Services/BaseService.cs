using Moz.Bus.Dtos;

namespace Moz.Bus.Services
{
    public class BaseService
    {
        protected OkResult Ok()
        {
            return new OkResult();
        }

        protected ErrorResult Error(string msg = "发生错误", int code = 600)
        {
            return new ErrorResult(msg, code);
        }
    }
}