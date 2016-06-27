﻿using log4net;
using System;
using Microsoft.Extensions.Logging;

namespace Log4net.Extensions.Logging
{
    public class Log4NetAdapter : Microsoft.Extensions.Logging.ILogger
    {
        private readonly ILog _logger;

        public Log4NetAdapter(string loggerName)
        {
            _logger = LogManager.GetLogger(loggerName);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Trace:
                case LogLevel.Debug:
                    return _logger.IsDebugEnabled;
                case LogLevel.Information:
                    return _logger.IsInfoEnabled;
                case LogLevel.Warning:
                    return _logger.IsWarnEnabled;
                case LogLevel.Error:
                    return _logger.IsErrorEnabled;
                case LogLevel.Critical:
                    return _logger.IsFatalEnabled;
                case LogLevel.None:
                    return false;
                default:
                    throw new ArgumentException($"Unknown log level {logLevel}.", nameof(logLevel));
            }
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
            Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }
            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            var message = formatter(state, exception);

            switch (logLevel)
            {
                case LogLevel.Trace:
                case LogLevel.Debug:
                    _logger.Debug(message, exception);
                    break;
                case LogLevel.Information:
                    _logger.Info(message, exception);
                    break;
                case LogLevel.Warning:
                    _logger.Warn(message, exception);
                    break;
                case LogLevel.Error:
                    _logger.Error(message, exception);
                    break;
                case LogLevel.Critical:
                    _logger.Fatal(message, exception);
                    break;
                case LogLevel.None:
                    break;
                default:
                    _logger.Warn($"Encountered unknown log level {logLevel}, writing out as Info.");
                    _logger.Info(message, exception);
                    break;
            }
        }
    }
}