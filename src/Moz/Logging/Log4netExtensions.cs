using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Moz.Logging
{
	/// <summary>
	/// The log4net extensions class.
	/// </summary>
	public static class Log4NetExtensions
	{
		/// <summary>
        /// Adds the log4net logging provider.
        /// </summary>
        /// <param name="builder">The logging builder instance.</param>
        /// <returns>The <see ref="ILoggingBuilder" /> passed as parameter with the new provider registered.</returns>
        public static ILoggingBuilder AddLog4Net(this ILoggingBuilder builder)
        {
			var options = new Log4NetProviderOptions();
			return builder.AddLog4Net(options);
        }

        /// <summary>
        /// Adds the log4net logging provider.
        /// </summary>
        /// <param name="builder">The logging builder instance.</param>
        /// <param name="log4NetConfigFile">The log4net Config File.</param>
        /// <returns>The <see ref="ILoggingBuilder" /> passed as parameter with the new provider registered.</returns>
        public static ILoggingBuilder AddLog4Net(this ILoggingBuilder builder, string log4NetConfigFile)
        {
			var options = new Log4NetProviderOptions(log4NetConfigFile);
			return builder.AddLog4Net(options);
		}

        /// <summary>
        /// Adds the log4net logging provider.
        /// </summary>
        /// <param name="builder">The logging builder instance.</param>
        /// <param name="log4NetConfigFile">The log4net Config File.</param>
        /// <param name="watch">if set to <c>true</c>, the configuration will be reloaded when the xml configuration file changes.</param>
        /// <returns>
        /// The <see ref="ILoggingBuilder" /> passed as parameter with the new provider registered.
        /// </returns>
        public static ILoggingBuilder AddLog4Net(this ILoggingBuilder builder, string log4NetConfigFile, bool watch)
        {
			var options = new Log4NetProviderOptions(log4NetConfigFile, watch);
			return builder.AddLog4Net(options);
		}

        /// <summary>
        /// Adds the log4net logging provider.
        /// </summary>
        /// <param name="builder">The logging builder instance.</param>
        /// <param name="log4NetConfigFile">The log4net Config File.</param>
        /// <returns>The <see ref="ILoggingBuilder" /> passed as parameter with the new provider registered.</returns>
        public static ILoggingBuilder AddLog4Net(this ILoggingBuilder builder, Log4NetProviderOptions options)
        {
			builder.Services.AddSingleton<ILoggerProvider>(new Log4NetProvider(options));
            return builder; 
        }
	}
}