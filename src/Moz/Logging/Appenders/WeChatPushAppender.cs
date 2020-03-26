
using log4net.Appender;
using log4net.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace Moz.Logging.Appenders
{
    public class WeChatPushAppender:AppenderSkeleton
    {
        protected override void Append(LoggingEvent loggingEvent)
        {
            throw new System.NotImplementedException();
        }
    }
}