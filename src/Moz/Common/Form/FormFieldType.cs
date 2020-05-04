using System;
using System.ComponentModel.DataAnnotations;

namespace Moz.Common.Form
{
    [Flags]
    public enum FormFieldType
    {
        /// <summary>
        /// 单行文本
        /// </summary>
        [Display(Name = "单行文本")] 
        SingleRowTextInput = 1,

        /// <summary>
        /// 下拉列表
        /// </summary>
        [Display(Name = "下拉列表")] 
        Select = 2,

        /// <summary>
        /// 单选按钮
        /// </summary>
        [Display(Name = "单选按钮")] 
        Radio = 4,

        /// <summary>
        /// 日期
        /// </summary>
        [Display(Name = "日期")] 
        DateTimeInput = 8,

        /// <summary>
        /// 复选框
        /// </summary>
        [Display(Name = "复选框")] 
        CheckBox = 16,

        /// <summary>
        /// 颜色
        /// </summary>
        [Display(Name = "颜色")] 
        Color = 32,

        /// <summary>
        /// 编辑器
        /// </summary>
        [Display(Name = "编辑器")] 
        Editor = 64,

        /// <summary>
        /// 单文件上传
        /// </summary>
        [Display(Name = "单文件上传")] 
        UploadSingleFile = 128,

        /// <summary>
        /// 多文件上传
        /// </summary>
        [Display(Name = "多文件上传")] 
        UploadMultipleFiles = 256,

        /// <summary>
        /// 多行文本
        /// </summary>
        [Display(Name = "多行文本")] 
        MultipleRowsTextInput = 512,

        /// <summary>
        /// 地理位置
        /// </summary>
        [Display(Name = "地理位置")] 
        Location = 1024,

        /// <summary>
        /// 类别选择
        /// </summary>
        [Display(Name = "切换按钮")] 
        Switch = 2048
    }
}