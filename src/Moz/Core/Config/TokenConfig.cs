namespace Moz.Core.Config
{
    public class TokenConfig
    {
        /// <summary>
        /// 
        /// </summary>
        public string Issuer { get; set; } = "https://ut32.com";

        /// <summary>
        /// 
        /// </summary>
        public string Audience { get; set; } = "moz_application";

        /// <summary>
        /// 过期(以天为单位)
        /// </summary>
        public int Expire { get; set; } = 30;
    }
}