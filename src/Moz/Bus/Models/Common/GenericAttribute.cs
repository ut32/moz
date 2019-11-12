using Moz.Bus.Models;
using Moz.CMS.Models;
using Moz.CMS.Models;
using SqlSugar;

namespace Moz.CMS.Model.Common
{
    /// <summary>
    ///     generic_attribute
    /// </summary>
    [SugarTable("tab_generic_attribute")]
    public class GenericAttribute : BaseModel
    {
        #region 属性

        /// <summary>
        /// </summary>
        [SugarColumn(ColumnName = "entity_id")]
        public long EntityId { get; set; }

        /// <summary>
        /// </summary>
        [SugarColumn(ColumnName = "key_group")]
        public string KeyGroup { get; set; }

        /// <summary>
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// </summary>
        public string Value { get; set; }

        #endregion
    }
}