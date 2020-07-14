using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Moz.Bus.Models.Common;
using Moz.Common.Form;
using Moz.FileStorage;

namespace Moz.Settings
{

    /// <summary>
    /// 全局设置
    /// </summary>
    [Display(Name = "全局设置", Order = -99999)]
    public class GlobalSettings : ISettings
    {
        public GlobalSettings()
        { 
        }
        
        /// <summary>
        /// 站点标题
        /// </summary>
        [FormItem(Name = "站点标题", Order = 10)]
        public string SiteTitle { get; set; } 
        
        /// <summary>
        /// 站点地址(Url)
        /// </summary>
        [FormItem(Name = "站点地址(Url)", Order = 20)]
        public string SiteUrl { get; set; }
        
        /// <summary>
        /// 关闭站点
        /// </summary>
        [FormItem(Name = "关闭站点", Order = 30)]
        public bool DisableSite { get; set; } 

        /// <summary>
        /// 用户重置密码
        /// </summary>
        [FormItem(Name = "用户重置密码", Order = 40)]
        public string ResetPassword { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [FormItem(Name = "文件上传器", 
            Order = 50, 
            FieldType = FormFieldType.Select, 
            DataSource = typeof(FileStorageDataSource),
            Description = " 如选本地上传，文件会上传到本地wwwroot文件夹")]
        public string FileUploader { get; set; } 
    }
}