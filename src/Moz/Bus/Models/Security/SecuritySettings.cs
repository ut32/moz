using Moz.Configuration;

namespace Moz.Domain.Models.Security
{
    public class SecuritySettings : ISettings
    {
        /// <summary>
        ///     Gets or sets a value indicating whether all pages will be forced to use SSL (no matter of a specified
        ///     [HttpsRequirementAttribute] attribute)
        /// </summary>
        public bool ForceSsl { get; set; }

        /// <summary>
        ///     Gets or sets an encryption key
        /// </summary>
        public string EncryptionKey { get; set; }
    }
}