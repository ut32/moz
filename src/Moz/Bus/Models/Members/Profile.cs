using Moz.Bus.Models;
using SqlSugar;

namespace Moz.CMS.Models.Members
{
    /// <summary>
    ///     profile
    /// </summary>
    [SugarTable("tab_profile")]
    public class Profile : BaseModel
    {
        #region 属性

        /// <summary>
        /// </summary>
        [SugarColumn(ColumnName = "header_bg")]
        public string HeaderBg { get; set; }

        /// <summary>
        /// </summary>
        public string Intro { get; set; }

        /// <summary>
        /// </summary>
        [SugarColumn(ColumnName = "like_gender")]
        public sbyte LikeGender { get; set; }

        /// <summary>
        /// </summary>
        [SugarColumn(ColumnName = "like_age")]
        public string LikeAge { get; set; }

        /// <summary>
        /// </summary>
        public string Wechat { get; set; }

        /// <summary>
        /// </summary>
        public string Qq { get; set; }

        /// <summary>
        /// </summary>
        public string Twitter { get; set; }

        /// <summary>
        /// </summary>
        public int Weight { get; set; }

        /// <summary>
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// </summary>
        public sbyte Income { get; set; }

        /// <summary>
        /// </summary>
        public sbyte Religion { get; set; }

        /// <summary>
        /// </summary>
        [SugarColumn(ColumnName = "i_viewed")]
        public int iViewed { get; set; }

        /// <summary>
        /// </summary>
        [SugarColumn(ColumnName = "i_liked")]
        public int iLiked { get; set; }

        /// <summary>
        /// </summary>
        [SugarColumn(ColumnName = "i_gift_presented")]
        public int iGiftPresented { get; set; }

        /// <summary>
        /// </summary>
        [SugarColumn(ColumnName = "viewed_me")]
        public int ViewedMe { get; set; }

        /// <summary>
        /// </summary>
        [SugarColumn(ColumnName = "liked_me")]
        public int LikedMe { get; set; }

        /// <summary>
        /// </summary>
        [SugarColumn(ColumnName = "gift_prensented_to_me")]
        public int GiftPrensentedToMe { get; set; }

        #endregion
    }
}