using System.IO;
using Microsoft.Extensions.Logging;

namespace Moz.Logging
{
    // ReSharper disable once InconsistentNaming
    public class Log4netProvider : ILoggerProvider
    {
        private readonly FileInfo _fileInfo;

        // ReSharper disable once InconsistentNaming
        public Log4netProvider(string log4netConfigFile)
        {
            _fileInfo = new FileInfo(log4netConfigFile);
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new Log4netLogger(categoryName, _fileInfo);
        }

        public void Dispose()
        {
        }
    }
}