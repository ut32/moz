using Moz.Bus.Models;
using Moz.CMS.Models;
using Moz.CMS.Models;
using SqlSugar;

namespace Moz.CMS.Model.Localization
{
    /// <summary>
    ///     localized_property
    /// </summary>
    [SugarTable("tab_localized_property")]
    public class LocalizedProperty : BaseModel
    {
        #region 属性

        /// <summary>
        /// </summary>
        [SugarColumn(ColumnName = "entity_id")]
        public long EntityId { get; set; }

        /// <summary>
        /// </summary>
        [SugarColumn(ColumnName = "language_id")]
        public long LanguageId { get; set; }

        /// <summary>
        /// </summary>
        [SugarColumn(ColumnName = "locale_key_group")]
        public string LocaleKeyGroup { get; set; }

        /// <summary>
        /// </summary>
        [SugarColumn(ColumnName = "locale_key")]
        public string LocaleKey { get; set; }

        /// <summary>
        /// </summary>
        [SugarColumn(ColumnName = "locale_value")]
        public string LocaleValue { get; set; }

        #endregion
    }
}