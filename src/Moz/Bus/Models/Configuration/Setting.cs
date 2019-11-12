using Moz.Bus.Models;
using Moz.CMS.Models;
using Moz.CMS.Model.Localization;
using Moz.CMS.Models;
using SqlSugar;

namespace Moz.CMS.Model.Configuration
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