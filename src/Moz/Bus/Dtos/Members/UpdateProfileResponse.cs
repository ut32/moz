using Moz.Bus.Dtos;
using Moz.CMS.Dtos;
using Moz.CMS.Dtos;
using Moz.Domain.Dtos;

namespace Moz.Core.Dtos.Members
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class UpdateProfileResponse : IRespData
    {
        public int Code { get; set; }
        public string Message { get; set; }
    }
}