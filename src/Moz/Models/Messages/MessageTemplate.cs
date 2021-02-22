using Moz.Bus.Models.Localization;
using Moz.Models.Messages;

namespace Moz.Bus.Models.Messages
{
    public class MessageTemplate : BaseModel, ILocalizedEntity
    {
        /// <summary>
        ///     Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the BCC Email addresses
        /// </summary>
        public string BccEmailAddresses { get; set; }

        /// <summary>
        ///     Gets or sets the subject
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        ///     Gets or sets the body
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the template is active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        ///     Gets or sets the delay before sending message
        /// </summary>
        public int? DelayBeforeSend { get; set; }

        /// <summary>
        ///     Gets or sets the period of message delay
        /// </summary>
        public int DelayPeriodId { get; set; }

        /// <summary>
        ///     Gets or sets the download identifier of attached file
        /// </summary>
        public int AttachedDownloadId { get; set; }

        /// <summary>
        ///     Gets or sets the used email account identifier
        /// </summary>
        public int EmailAccountId { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the entity is limited/restricted to certain stores
        /// </summary>
        public bool LimitedToStores { get; set; }

        /// <summary>
        ///     Gets or sets the period of message delay
        /// </summary>
        public MessageDelayPeriod DelayPeriod
        {
            get => (MessageDelayPeriod) DelayPeriodId;
            set => DelayPeriodId = (int) value;
        }
    }
}