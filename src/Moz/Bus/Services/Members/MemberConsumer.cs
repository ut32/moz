using System;
using System.Threading;
using Moz.Bus.Models.Members;
using Moz.CMS.Models.Members;
using Moz.Events;

namespace Moz.Domain.Services.Members
{
    public class MemberConsumer : ISubscriber<EntityCreated<Member>>
    {
        public void HandleEvent(EntityCreated<Member> eventMessage, dynamic opts)
        {
            throw new NotImplementedException();
        }

        public void HandleEvent(EntityCreated<Member> eventMessage)
        {
            var member = eventMessage.Entity;
            Thread.Sleep(10000);
            var id = member.Id;
        }
    }
}