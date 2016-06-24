﻿using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Log4net.Extensions.Logging
{
    public class Log4NetProvider : ILoggerProvider
    {
        private IDictionary<string, ILogger> _loggers
            = new Dictionary<string, ILogger>();

        public ILogger CreateLogger(string name)
        {
            if (!_loggers.ContainsKey(name))
            {
                lock (_loggers)
                {
                    // Have to check again since another thread may have gotten the lock first
                    if (!_loggers.ContainsKey(name))
                    {
                        _loggers[name] = new Log4NetAdapter(name);
                    }
                }
            }
            return _loggers[name];
        }

        public void Dispose()
        {
            _loggers.Clear();
            _loggers = null;
        }
    }
}