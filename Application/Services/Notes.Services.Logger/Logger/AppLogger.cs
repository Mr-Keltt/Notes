using System;
using Serilog;
using Serilog.Events;

namespace Notes.Services.Logger
{
    /// <summary>
    /// Provides a standardized logging interface built upon Serilog for consistent log message formatting.
    /// </summary>
    public class AppLogger : IAppLogger
    {
        // Underlying Serilog logger instance used for outputting log messages.
        private readonly ILogger logger;

        // Identifier for the system to be prepended to each log message.
        private string _systemName = "Notes";

        /// <summary>
        /// Initializes a new instance of the AppLogger class with a specified Serilog logger.
        /// </summary>
        /// <param name="logger">An instance of Serilog's ILogger to use for logging.</param>
        public AppLogger(ILogger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Constructs a standardized log message by prepending the system name and optional module context.
        /// </summary>
        /// <param name="messageTemplate">The base message template with placeholders.</param>
        /// <param name="module">Optional module object whose type name will be included if provided.</param>
        /// <returns>A formatted log message template string.</returns>
        private string consructMessageTemplate(string messageTemplate, object module = null)
        {
            var moduleName = module?.GetType().Name;
            if (string.IsNullOrEmpty(moduleName))
                return $"[{_systemName}] {messageTemplate} ";
            else
                return $"[{_systemName}:{module}] {messageTemplate} ";
        }

        /// <summary>
        /// Logs a message at the specified severity level.
        /// </summary>
        /// <param name="level">The log event severity level.</param>
        /// <param name="messageTemplate">A message template that may contain format placeholders.</param>
        /// <param name="propertyValues">Values that will replace the placeholders in the message template.</param>
        public void Write(LogEventLevel level, string messageTemplate, params object[] propertyValues)
        {
            logger?.Write(level, consructMessageTemplate(messageTemplate), propertyValues);
        }

        /// <summary>
        /// Logs a message at the specified severity level, including module context.
        /// </summary>
        /// <param name="level">The log event severity level.</param>
        /// <param name="module">An object representing module context to include in the log message.</param>
        /// <param name="messageTemplate">A message template that may contain format placeholders.</param>
        /// <param name="propertyValues">Values to replace the placeholders in the message template.</param>
        public void Write(LogEventLevel level, object module, string messageTemplate, params object[] propertyValues)
        {
            logger?.Write(level, consructMessageTemplate(messageTemplate, module), propertyValues);
        }

        /// <summary>
        /// Logs a message along with an exception at the specified severity level.
        /// </summary>
        /// <param name="level">The log event severity level.</param>
        /// <param name="exception">The exception to log details from.</param>
        /// <param name="messageTemplate">A message template that may contain format placeholders.</param>
        /// <param name="propertyValues">Values to replace the placeholders in the message template.</param>
        public void Write(LogEventLevel level, Exception exception, string messageTemplate, params object[] propertyValues)
        {
            logger?.Write(level, exception, consructMessageTemplate(messageTemplate), propertyValues);
        }

        /// <summary>
        /// Logs a message with an exception and module context at the specified severity level.
        /// </summary>
        /// <param name="level">The log event severity level.</param>
        /// <param name="exception">The exception to log details from.</param>
        /// <param name="module">An object representing module context to include in the log message.</param>
        /// <param name="messageTemplate">A message template that may contain format placeholders.</param>
        /// <param name="propertyValues">Values to replace the placeholders in the message template.</param>
        public void Write(LogEventLevel level, Exception exception, object module, string messageTemplate, params object[] propertyValues)
        {
            logger?.Write(level, exception, consructMessageTemplate(messageTemplate, module), propertyValues);
        }

        /// <summary>
        /// Logs a verbose-level message.
        /// </summary>
        /// <param name="messageTemplate">A message template that may contain format placeholders.</param>
        /// <param name="propertyValues">Values to replace the placeholders in the message template.</param>
        public void Verbose(string messageTemplate, params object[] propertyValues)
        {
            logger?.Verbose(consructMessageTemplate(messageTemplate), propertyValues);
        }

        /// <summary>
        /// Logs a verbose-level message with module context.
        /// </summary>
        /// <param name="module">An object representing module context to include in the log message.</param>
        /// <param name="messageTemplate">A message template that may contain format placeholders.</param>
        /// <param name="propertyValues">Values to replace the placeholders in the message template.</param>
        public void Verbose(object module, string messageTemplate, params object[] propertyValues)
        {
            logger?.Verbose(consructMessageTemplate(messageTemplate, module), propertyValues);
        }

        /// <summary>
        /// Logs a debug-level message.
        /// </summary>
        /// <param name="messageTemplate">A message template that may contain format placeholders.</param>
        /// <param name="propertyValues">Values to replace the placeholders in the message template.</param>
        public void Debug(string messageTemplate, params object[] propertyValues)
        {
            logger?.Debug(consructMessageTemplate(messageTemplate), propertyValues);
        }

        /// <summary>
        /// Logs a debug-level message with module context.
        /// </summary>
        /// <param name="module">An object representing module context to include in the log message.</param>
        /// <param name="messageTemplate">A message template that may contain format placeholders.</param>
        /// <param name="propertyValues">Values to replace the placeholders in the message template.</param>
        public void Debug(object module, string messageTemplate, params object[] propertyValues)
        {
            logger?.Debug(consructMessageTemplate(messageTemplate, module), propertyValues);
        }

        /// <summary>
        /// Logs an informational-level message.
        /// </summary>
        /// <param name="messageTemplate">A message template that may contain format placeholders.</param>
        /// <param name="propertyValues">Values to replace the placeholders in the message template.</param>
        public void Information(string messageTemplate, params object[] propertyValues)
        {
            logger?.Information(consructMessageTemplate(messageTemplate), propertyValues);
        }

        /// <summary>
        /// Logs an informational-level message with module context.
        /// </summary>
        /// <param name="module">An object representing module context to include in the log message.</param>
        /// <param name="messageTemplate">A message template that may contain format placeholders.</param>
        /// <param name="propertyValues">Values to replace the placeholders in the message template.</param>
        public void Information(object module, string messageTemplate, params object[] propertyValues)
        {
            logger?.Information(consructMessageTemplate(messageTemplate, module), propertyValues);
        }

        /// <summary>
        /// Logs a warning-level message.
        /// </summary>
        /// <param name="messageTemplate">A message template that may contain format placeholders.</param>
        /// <param name="propertyValues">Values to replace the placeholders in the message template.</param>
        public void Warning(string messageTemplate, params object[] propertyValues)
        {
            logger?.Warning(consructMessageTemplate(messageTemplate), propertyValues);
        }

        /// <summary>
        /// Logs a warning-level message with module context.
        /// </summary>
        /// <param name="module">An object representing module context to include in the log message.</param>
        /// <param name="messageTemplate">A message template that may contain format placeholders.</param>
        /// <param name="propertyValues">Values to replace the placeholders in the message template.</param>
        public void Warning(object module, string messageTemplate, params object[] propertyValues)
        {
            logger?.Warning(consructMessageTemplate(messageTemplate, module), propertyValues);
        }

        /// <summary>
        /// Logs an error-level message.
        /// </summary>
        /// <param name="messageTemplate">A message template that may contain format placeholders.</param>
        /// <param name="propertyValues">Values to replace the placeholders in the message template.</param>
        public void Error(string messageTemplate, params object[] propertyValues)
        {
            logger?.Error(consructMessageTemplate(messageTemplate), propertyValues);
        }

        /// <summary>
        /// Logs an error-level message with module context.
        /// </summary>
        /// <param name="module">An object representing module context to include in the log message.</param>
        /// <param name="messageTemplate">A message template that may contain format placeholders.</param>
        /// <param name="propertyValues">Values to replace the placeholders in the message template.</param>
        public void Error(object module, string messageTemplate, params object[] propertyValues)
        {
            logger?.Error(consructMessageTemplate(messageTemplate, module), propertyValues);
        }

        /// <summary>
        /// Logs an error-level message along with an exception.
        /// </summary>
        /// <param name="exception">The exception to capture details from.</param>
        /// <param name="messageTemplate">A message template that may contain format placeholders.</param>
        /// <param name="propertyValues">Values to replace the placeholders in the message template.</param>
        public void Error(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            logger?.Error(exception, consructMessageTemplate(messageTemplate), propertyValues);
        }

        /// <summary>
        /// Logs an error-level message with an exception and module context.
        /// </summary>
        /// <param name="exception">The exception to capture details from.</param>
        /// <param name="module">An object representing module context to include in the log message.</param>
        /// <param name="messageTemplate">A message template that may contain format placeholders.</param>
        /// <param name="propertyValues">Values to replace the placeholders in the message template.</param>
        public void Error(Exception exception, object module, string messageTemplate, params object[] propertyValues)
        {
            logger?.Error(exception, consructMessageTemplate(messageTemplate, module), propertyValues);
        }

        /// <summary>
        /// Logs a fatal-level message.
        /// </summary>
        /// <param name="messageTemplate">A message template that may contain format placeholders.</param>
        /// <param name="propertyValues">Values to replace the placeholders in the message template.</param>
        public void Fatal(string messageTemplate, params object[] propertyValues)
        {
            logger?.Fatal(consructMessageTemplate(messageTemplate), propertyValues);
        }

        /// <summary>
        /// Logs a fatal-level message with module context.
        /// </summary>
        /// <param name="module">An object representing module context to include in the log message.</param>
        /// <param name="messageTemplate">A message template that may contain format placeholders.</param>
        /// <param name="propertyValues">Values to replace the placeholders in the message template.</param>
        public void Fatal(object module, string messageTemplate, params object[] propertyValues)
        {
            logger?.Fatal(consructMessageTemplate(messageTemplate, module), propertyValues);
        }

        /// <summary>
        /// Logs a fatal-level message along with an exception.
        /// </summary>
        /// <param name="exception">The exception to capture details from.</param>
        /// <param name="messageTemplate">A message template that may contain format placeholders.</param>
        /// <param name="propertyValues">Values to replace the placeholders in the message template.</param>
        public void Fatal(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            logger?.Fatal(exception, consructMessageTemplate(messageTemplate), propertyValues);
        }

        /// <summary>
        /// Logs a fatal-level message with an exception and module context.
        /// </summary>
        /// <param name="exception">The exception to capture details from.</param>
        /// <param name="module">An object representing module context to include in the log message.</param>
        /// <param name="messageTemplate">A message template that may contain format placeholders.</param>
        /// <param name="propertyValues">Values to replace the placeholders in the message template.</param>
        public void Fatal(Exception exception, object module, string messageTemplate, params object[] propertyValues)
        {
            logger?.Fatal(exception, consructMessageTemplate(messageTemplate, module), propertyValues);
        }
    }
}
