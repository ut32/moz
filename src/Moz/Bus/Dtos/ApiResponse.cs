// ReSharper disable InconsistentNaming

using Moz.Bus.Dtos;

namespace Moz.CMS.Dtos
{
    public class ApiResponse<T> where T : IRespData
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public string SignType { get; set; }
        public string Sign { get; set; }
        public T RespData { get; set; }
    } 
}