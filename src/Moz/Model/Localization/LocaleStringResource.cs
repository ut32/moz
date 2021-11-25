﻿using SqlSugar;

namespace Moz.Bus.Models.Localization
{
    /// <summary>
    ///     locale_string_resource
    /// </summary>
    [SugarTable("tab_locale_string_resource")]
    public class LocaleStringResource : BaseModel
    {
        #region 属性

        /// <summary>
        /// </summary>
        [SugarColumn(ColumnName = "language_id")]
        public long LanguageId { get; set; }

        /// <summary>
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// </summary>
        public string Value { get; set; }

        #endregion
    }
}