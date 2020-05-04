using System;
using System.Collections.Generic;

namespace Moz.Common.Form
{
    public class FormItemAttribute:Attribute
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }
        
        /// <summary>
        /// 类型
        /// </summary>
        public FormFieldType FieldType { get; set; } 

        /// <summary>
        /// 数据源
        /// </summary>
        public Type DataSource { get; set; }
        
        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultValue { get; set; }
    }
}