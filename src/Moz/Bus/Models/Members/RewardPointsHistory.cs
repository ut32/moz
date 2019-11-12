using System;
using Moz.Bus.Models;
using SqlSugar;

namespace Moz.CMS.Models.Members
{
    /// <summary>
    ///     reward_points_history
    /// </summary>
    [SugarTable("tab_reward_points_history")]
    public class RewardPointsHistory : BaseModel
    {
        #region 属性

        /// <summary>
        /// </summary>
        [SugarColumn(ColumnName = "member_id")]
        public long MemberId { get; set; }

        /// <summary>
        /// </summary>
        public int Points { get; set; }

        /// <summary>
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// </summary>
        [SugarColumn(ColumnName = "points_balance")]
        public int PointsBalance { get; set; }

        /// <summary>
        /// </summary>
        [SugarColumn(ColumnName = "add_time")]
        public DateTime AddTime { get; set; }

        #endregion
    }
}