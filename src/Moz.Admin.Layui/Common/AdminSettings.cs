using System.ComponentModel.DataAnnotations;
using Moz.Common.Form;
using Moz.Settings;

namespace Moz.Admin.Layui.Common
{
    [Display(Name = "后台设置", Order = -99998)]
    public class AdminSettings : ISettings
    {
        /// <summary>
        /// 后台标题
        /// </summary>
        [FormItem(Name = "后台标题", Order = 50)]
        public string AdminTitle { get; set; }
        
        /// <summary>
        /// 后台header
        /// </summary>
        [FormItem(Name = "后台header", Description = "设置后台主题", Order = 60, FieldType = FormFieldType.MultipleRowsTextInput)]
        public string AdminHeader { get; set; }
        
        /// <summary>
        /// 后台footer
        /// </summary>
        [FormItem(Name = "后台footer", Order = 70, FieldType = FormFieldType.MultipleRowsTextInput)]
        public string AdminFooter { get; set; }
        
        /// <summary>
        /// 自定义后台首页
        /// </summary>
        [FormItem(Name = "后台欢迎页", Order = 80)]
        public string AdminWelcomeView { get; set; }
        
        /// <summary>
        /// 自定义登录页
        /// </summary>
        [FormItem(Name = "后台登录页", Order = 90)]
        public string AdminLoginView { get; set; }
    }
}