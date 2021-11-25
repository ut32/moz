using SqlSugar;

namespace Moz.Bus.Models.Localization
{
    /// <summary>
    ///     language
    /// </summary>
    [SugarTable("tab_language")]
    public class Language : BaseModel
    {
        #region 属性

        /// <summary>
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// </summary>
        [SugarColumn(ColumnName = "language_culture")]
        public string LanguageCulture { get; set; }

        /// <summary>
        /// </summary>
        public bool Published { get; set; }

        #endregion
    }
}