using Moz.Bus.Models.Localization;
using Moz.Bus.Models.Members;

namespace Moz.Core
{
    public interface IWorkContext
    {
        /// <summary>
        /// 当前用户
        /// </summary>
        SimpleMember CurrentMember { get; }
        
        /// <summary>
        /// 语言
        /// </summary>
        Language WorkingLanguage { get; }
    }
}