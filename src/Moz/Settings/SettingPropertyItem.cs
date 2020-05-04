using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Moz.Common.Form;

namespace Moz.Settings
{
    public class SettingPropertyItem
    {
        /// <summary>
        /// Property名称
        /// </summary>
        public string TypeName { get;  } 
        
        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get;  }
        
        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get;  }
        
        /// <summary>
        /// 字段类型
        /// </summary>
        public FormFieldType FieldType { get;  } 
        
        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }
        
        /// <summary>
        /// 数据源
        /// </summary>
        public Dictionary<string,string> Data { get; }

        public SettingPropertyItem(PropertyInfo propertyInfo)
        {
            TypeName = propertyInfo.Name;
            DisplayName = propertyInfo.Name;
            FieldType = GetFormFieldType(propertyInfo);
            Data = new Dictionary<string, string>();
            
            var formItemAttr = propertyInfo.GetCustomAttribute<FormItemAttribute>();
            if (formItemAttr != null)
            {
                DisplayName = formItemAttr.Name;
                Order = formItemAttr.Order;
                FieldType = formItemAttr.FieldType;
                if (FieldType == default)
                {
                    FieldType = GetFormFieldType(propertyInfo);
                }

                if (formItemAttr.DataSource != null)
                {
                    if (formItemAttr.DataSource.IsEnum)
                    {
                        FieldType = FormFieldType.Select;
                        foreach (var fieldInfo in formItemAttr.DataSource.GetFields())
                        {
                            if (fieldInfo.FieldType.IsEnum)
                            {
                                var descAttr = fieldInfo.GetCustomAttribute<DescriptionAttribute>();
                                var @enum = Enum.Parse(formItemAttr.DataSource, fieldInfo.Name, true);
                                Data.Add(descAttr?.Description ?? fieldInfo.Name, ((int) @enum).ToString());
                            }
                        }
                    }
                }
            }
            else
            {
                var displayAttr = propertyInfo.GetCustomAttribute<DisplayAttribute>();
                if (displayAttr != null)
                {
                    DisplayName = displayAttr.Name;
                    try
                    {
                        Order = displayAttr.Order;
                    }
                    catch (Exception ex)
                    {
                        // ignored
                    }
                }
                else
                {
                    var descAttr = propertyInfo.GetCustomAttribute<DescriptionAttribute>();
                    if (descAttr != null)
                    {
                        DisplayName = descAttr.Description;
                    }
                }
            }
        }

        private FormFieldType GetFormFieldType(PropertyInfo propertyInfo)
        {
            if (propertyInfo.PropertyType == typeof(string)) return FormFieldType.SingleRowTextInput;

            if (propertyInfo.PropertyType == typeof(bool)) return FormFieldType.Switch;

            if (propertyInfo.PropertyType == typeof(DateTime)) return FormFieldType.DateTimeInput;

            if (propertyInfo.PropertyType == typeof(int)) return FormFieldType.SingleRowTextInput;

            if (propertyInfo.PropertyType == typeof(float)) return FormFieldType.SingleRowTextInput;

            if (propertyInfo.PropertyType == typeof(double)) return FormFieldType.SingleRowTextInput;

            if (propertyInfo.PropertyType == typeof(decimal)) return FormFieldType.SingleRowTextInput;

            return FormFieldType.SingleRowTextInput;
        }
    }
}