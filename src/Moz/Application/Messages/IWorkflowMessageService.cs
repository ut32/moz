using System.Collections.Generic;
using Moz.Bus.Models.Members;
using Moz.Model;

namespace Moz.Service.Messages
{
    public interface IWorkflowMessageService
    {
        /// <summary>
        ///     Sends a test email
        /// </summary>
        /// <param name="messageTemplateId">Message template identifier</param>
        /// <param name="sendToEmail">Send to email</param>
        /// <param name="tokens">Tokens</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        ///int SendTestEmail(int messageTemplateId, string sendToEmail, List<Token> tokens, int languageId);

        #region Customer workflow

        /// <summary>
        ///     Sends the customer registered notification message.
        /// </summary>
        /// <returns>The customer registered notification message.</returns>
        /// <param name="customer">Customer.</param>
        /// <param name="languageId">Language identifier.</param>
        int SendCustomerRegisteredNotificationMessage(User customer, int languageId);

        /// <summary>
        ///     Sends the customer welcome message.
        /// </summary>
        /// <returns>The customer welcome message.</returns>
        /// <param name="customer">Customer.</param>
        /// <param name="languageId">Language identifier.</param>
        int SendCustomerWelcomeMessage(User customer, int languageId);

        /// <summary>
        ///     Sends an email validation message to a customer
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        int SendCustomerEmailValidationMessage(User customer, int languageId);

        /// <summary>
        ///     Sends an email re-validation message to a customer
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        int SendCustomerEmailRevalidationMessage(User customer, int languageId);

        /// <summary>
        ///     Sends password recovery message to a customer
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        int SendCustomerPasswordRecoveryMessage(User customer, int languageId);

        #endregion

        /// <summary>
        /// Send notification
        /// </summary>
        /// <param name="messageTemplate">Message template</param>
        /// <param name="emailAccount">Email account</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="tokens">Tokens</param>
        /// <param name="toEmailAddress">Recipient email address</param>
        /// <param name="toName">Recipient name</param>
        /// <param name="attachmentFilePath">Attachment file path</param>
        /// <param name="attachmentFileName">Attachment file name</param>
        /// <param name="replyToEmailAddress">"Reply to" email</param>
        /// <param name="replyToName">"Reply to" name</param>
        /// <param name="fromEmail">Sender email. If specified, then it overrides passed "emailAccount" details</param>
        /// <param name="fromName">Sender name. If specified, then it overrides passed "emailAccount" details</param>
        /// <param name="subject">Subject. If specified, then it overrides subject of a message template</param>
        /// <returns>Queued email identifier</returns>
        /*int SendNotification(MessageTemplate messageTemplate, EmailAccount emailAccount, int languageId, IEnumerable<Token> tokens,
            string toEmailAddress, string toName,
            string attachmentFilePath = null, string attachmentFileName = null,
            string replyToEmailAddress = null, string replyToName = null,
            string fromEmail = null, string fromName = null, string subject = null);*/
    }
}