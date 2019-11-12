// ReSharper disable InconsistentNaming

namespace Moz.Bus.Dtos
{
    public class BaseRespData : IRespData
    {
        public int Code { get; set; }
        public string Message { get; set; }
    }

    public interface IRespData
    {
        int Code { get; set; }
        string Message { get; set; }
    }
}