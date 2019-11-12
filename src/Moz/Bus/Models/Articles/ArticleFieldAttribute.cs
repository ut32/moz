using System;
using System.ComponentModel.DataAnnotations;

namespace Moz.Bus.Models.Articles
{
    [Flags]
    public enum FieldTypeEnum
    {
        [Display(Name = "单行文本")] SingleRowTextInput = 1,

        [Display(Name = "下拉列表")] Select = 2,

        [Display(Name = "单选按钮")] Radio = 4,

        [Display(Name = "日期")] DateTimeInput = 8,

        [Display(Name = "复选框")] CheckBox = 16,

        [Display(Name = "颜色")] Color = 32,

        [Display(Name = "编辑器")] Editor = 64,

        [Display(Name = "单文件上传")] UploadSingleFile = 128,

        [Display(Name = "多文件上传")] UploadMultipleFiles = 256,

        [Display(Name = "多文本")] MultipleRowsTextInput = 512,

        [Display(Name = "地理位置")] Location = 1024,

        [Display(Name = "类别选择")] Category = 2048
    }

    public class ArticleFieldAttribute : Attribute
    {
        public ArticleFieldAttribute(string name, FieldTypeEnum fieldType, bool multiLanguage = false)
        {
            Name = name;
            FieldType = fieldType;
            MultiLanguage = multiLanguage;
        }

        public FieldTypeEnum FieldType { get; }
        public string Name { get; }
        public bool MultiLanguage { get; }
    }
}