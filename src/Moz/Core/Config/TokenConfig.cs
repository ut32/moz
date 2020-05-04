namespace Moz.Core.Config
{
    public class TokenConfig
    {
        /// <summary>
        /// 默认为 https://ut32.com
        /// </summary>
        public string Issuer { get; set; } = "https://ut32.com";

        /// <summary>
        /// 默认为 moz_application
        /// </summary>
        public string Audience { get; set; } = "moz_application";

        /// <summary>
        /// 过期(以天为单位，默认为30天)
        /// </summary>
        public int Expire { get; set; } = 30;
    }
}