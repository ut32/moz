using System;
using SqlSugar;

namespace Moz.Bus.Models.Members
{
    /// <summary>
    /// user
    /// </summary>
    [SugarTable("tab_user")] 
    public class User : BaseModel
    {
        #region 属性
        public string UId { get; set; }

        /// <summary>
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// </summary>
        [SugarColumn(ColumnName = "password_salt")]
        public string PasswordSalt { get; set; }


        /// <summary>
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// </summary>
        public GenderEnum? Gender { get; set; }

        /// <summary>
        /// </summary>
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// </summary>
        [SugarColumn(ColumnName = "register_ip")]
        public string RegisterIp { get; set; }

        /// <summary>
        /// </summary>
        [SugarColumn(ColumnName = "register_datetime")]
        public DateTime RegisterDatetime { get; set; }

        /// <summary>
        /// </summary>
        [SugarColumn(ColumnName = "login_count")]
        public int LoginCount { get; set; }

        /// <summary>
        /// </summary>
        [SugarColumn(ColumnName = "last_login_ip")]
        public string LastLoginIp { get; set; }

        /// <summary>
        /// </summary>
        [SugarColumn(ColumnName = "last_login_datetime")]
        public DateTime LastLoginDatetime { get; set; }

        /// <summary>
        /// </summary>
        [SugarColumn(ColumnName = "cannot_login_until_date")]
        public DateTime? CannotLoginUntilDate { get; set; }

        /// <summary>
        /// </summary>
        [SugarColumn(ColumnName = "last_active_datetime")]
        public DateTime LastActiveDatetime { get; set; }

        /// <summary>
        /// </summary>
        [SugarColumn(ColumnName = "failed_login_attempts")]
        public int FailedLoginAttempts { get; set; }

        /// <summary>
        /// </summary>
        [SugarColumn(ColumnName = "online_time_count")]
        public int OnlineTimeCount { get; set; }

        /// <summary>
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// </summary>
        [SugarColumn(ColumnName = "region_code")]
        public string RegionCode { get; set; }

        /// <summary>
        /// </summary>
        public decimal? Lng { get; set; }

        /// <summary>
        /// </summary>
        public decimal? Lat { get; set; }

        /// <summary>
        /// </summary>
        public string Geohash { get; set; }

        /// <summary>
        /// </summary>
        [SugarColumn(ColumnName = "is_active")]
        public bool IsActive { get; set; }

        /// <summary>
        /// </summary>
        [SugarColumn(ColumnName = "is_delete")]
        public bool IsDelete { get; set; }

        /// <summary>
        /// </summary>
        [SugarColumn(ColumnName = "is_email_valid")]
        public bool IsEmailValid { get; set; }

        /// <summary>
        /// </summary>
        [SugarColumn(ColumnName = "is_mobile_valid")]
        public bool IsMobileValid { get; set; }

        #endregion
    }
}