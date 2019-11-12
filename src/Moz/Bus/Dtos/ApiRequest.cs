// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBeProtected.Global

namespace Moz.CMS.Dtos
{
    public class ApiRequest<T>
    { 
        public string ReqId { get; set; }
        public string Sign { get; set; }
        public string Time { get; set; }
        public T ReqData { get; set; }
    }
}