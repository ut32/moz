using Moz.Bus.Models.Localization;
using SqlSugar;

namespace Moz.Bus.Models.Configuration
{
    /// <summary>
    ///     setting
    /// </summary>
    [SugarTable("tab_setting")]
    public class Setting : BaseModel, ILocalizedEntity
    {
        #region 属性

        /// <summary>
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// </summary>
        public string Value { get; set; }

        #endregion
    }
}