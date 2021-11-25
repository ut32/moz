using System;
using System.Collections.Generic;
using Moz.Bus.Models.Members;
using Moz.Model;

namespace Moz.Service.Messages
{
    public class WorkflowMessageService : IWorkflowMessageService
    {
        public int SendCustomerRegisteredNotificationMessage(User customer, int languageId)
        {
            throw new NotImplementedException();
        }

        public int SendCustomerWelcomeMessage(User customer, int languageId)
        {
            throw new NotImplementedException();
        }

        public int SendCustomerEmailValidationMessage(User customer, int languageId)
        {
            throw new NotImplementedException();
        }

        public int SendCustomerEmailRevalidationMessage(User customer, int languageId)
        {
            throw new NotImplementedException();
        }

        public int SendCustomerPasswordRecoveryMessage(User customer, int languageId)
        {
            throw new NotImplementedException();
        }
    }
}