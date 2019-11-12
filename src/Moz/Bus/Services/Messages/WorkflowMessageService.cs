using System;
using System.Collections.Generic;
using Moz.Bus.Models.Members;
using Moz.CMS.Models.Members;
using Moz.Models.Members;

namespace Moz.Service.Messages
{
    public class WorkflowMessageService : IWorkflowMessageService
    {
        public int SendCustomerRegisteredNotificationMessage(Member customer, int languageId)
        {
            throw new NotImplementedException();
        }

        public int SendCustomerWelcomeMessage(Member customer, int languageId)
        {
            throw new NotImplementedException();
        }

        public int SendCustomerEmailValidationMessage(Member customer, int languageId)
        {
            throw new NotImplementedException();
        }

        public int SendCustomerEmailRevalidationMessage(Member customer, int languageId)
        {
            throw new NotImplementedException();
        }

        public int SendCustomerPasswordRecoveryMessage(Member customer, int languageId)
        {
            throw new NotImplementedException();
        }
    }
}