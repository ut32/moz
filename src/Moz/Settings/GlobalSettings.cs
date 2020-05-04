using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Moz.Bus.Models.Common;

namespace Moz.Settings
{

    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "全局设置", Order = -99999)]
    public class GlobalSettings : ISettings
    {
        public GlobalSettings()
        { 
        }
        
        [Display(Name="用户重置密码")]
        public string ResetPassword { get; set; }
    }
}