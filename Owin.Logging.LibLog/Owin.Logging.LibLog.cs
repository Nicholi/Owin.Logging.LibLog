#pragma warning disable 1591

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "YourRootNamespace.Logging")]

// If you copied this file manually, you need to change all "YourRootNameSpace" so not to clash with other libraries
// that use LibLog
#if LIBLOG_PROVIDERS_ONLY
namespace YourRootNamespace.LibLog
#else
namespace YourRootNamespace.Logging
#endif
{
    using System;
    using System.Diagnostics;
    using Microsoft.Owin.Logging;
    using Owin;

#if LIBLOG_PUBLIC
    public
#else
    internal
#endif
    class LibLogLoggerFactory : ILoggerFactory
    {
        protected readonly Func<String, ILog> m_LibLogConstructor;
        protected readonly Func<ILog, TraceEventType, bool> m_IsLogEventEnabledFunc;
        protected readonly Action<ILog, TraceEventType, String, Exception> m_WriteLogEventFunc;

        public LibLogLoggerFactory()
            : this(DefaultCreateLogger, DefaultIsLogEventEnabled, DefaultWriteLogEvent)
        {
        }

        public LibLogLoggerFactory(Func<ILog, TraceEventType, bool> isLogEventEnabledFunc)
            : this(DefaultCreateLogger, isLogEventEnabledFunc, DefaultWriteLogEvent)
        {
        }

        public LibLogLoggerFactory(Action<ILog, TraceEventType, String, Exception> writeLogEventFunc)
            : this(DefaultCreateLogger, DefaultIsLogEventEnabled, writeLogEventFunc)
        {
        }

        public LibLogLoggerFactory(Func<ILog, TraceEventType, bool> isLogEventEnabledFunc, Action<ILog, TraceEventType, String, Exception> writeLogEventFunc)
            : this(DefaultCreateLogger, isLogEventEnabledFunc, writeLogEventFunc)
        {
        }

        public LibLogLoggerFactory(Func<String, ILog> libLogConstructor, Func<ILog, TraceEventType, bool> isLogEventEnabledFunc, Action<ILog, TraceEventType, String, Exception> writeLogEventFunc)
        {
            if (libLogConstructor == null)
            {
                throw new ArgumentNullException(nameof(libLogConstructor));
            }
            if (isLogEventEnabledFunc == null)
            {
                throw new ArgumentNullException(nameof(isLogEventEnabledFunc));
            }
            if (writeLogEventFunc == null)
            {
                throw new ArgumentNullException(nameof(writeLogEventFunc));
            }

            m_LibLogConstructor = libLogConstructor;
            m_IsLogEventEnabledFunc = isLogEventEnabledFunc;
            m_WriteLogEventFunc = writeLogEventFunc;
        }

        public ILogger Create(String name)
        {
            return new LibLogLogger(m_LibLogConstructor(name), m_IsLogEventEnabledFunc, m_WriteLogEventFunc);
        }

        protected static ILog DefaultCreateLogger(String name)
        {
            return LogProvider.GetLogger(name);
        }

        protected static bool DefaultIsLogEventEnabled(ILog libLogger, TraceEventType traceEventType)
        {
            switch (traceEventType)
            {
                case TraceEventType.Critical:
                    return libLogger.IsFatalEnabled();
                case TraceEventType.Error:
                    return libLogger.IsErrorEnabled();
                case TraceEventType.Warning:
                    return libLogger.IsWarnEnabled();
                case TraceEventType.Information:
                    return libLogger.IsInfoEnabled();
                case TraceEventType.Verbose:
                    return libLogger.IsTraceEnabled();
                case TraceEventType.Start:
                    return libLogger.IsDebugEnabled();
                case TraceEventType.Stop:
                    return libLogger.IsDebugEnabled();
                case TraceEventType.Suspend:
                    return libLogger.IsDebugEnabled();
                case TraceEventType.Resume:
                    return libLogger.IsDebugEnabled();
                case TraceEventType.Transfer:
                    return libLogger.IsDebugEnabled();
                default:
                    throw new ArgumentOutOfRangeException(nameof(traceEventType), traceEventType, "Unhandled EventType");
            }
        }

        protected static void DefaultWriteLogEvent(ILog libLogger, TraceEventType traceEventType, String message, Exception ex)
        {
            if (ex != null)
            {
                WriteLogException(libLogger, traceEventType, message, ex);
                return;
            }

            switch (traceEventType)
            {
                case TraceEventType.Critical:
                    libLogger.FatalFormat(message);
                    break;
                case TraceEventType.Error:
                    libLogger.ErrorFormat(message);
                    break;
                case TraceEventType.Warning:
                    libLogger.WarnFormat(message);
                    break;
                case TraceEventType.Information:
                    libLogger.InfoFormat(message);
                    break;
                case TraceEventType.Verbose:
                    libLogger.TraceFormat(message);
                    break;
                case TraceEventType.Start:
                    libLogger.DebugFormat(message);
                    break;
                case TraceEventType.Stop:
                    libLogger.DebugFormat(message);
                    break;
                case TraceEventType.Suspend:
                    libLogger.DebugFormat(message);
                    break;
                case TraceEventType.Resume:
                    libLogger.DebugFormat(message);
                    break;
                case TraceEventType.Transfer:
                    libLogger.DebugFormat(message);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(traceEventType), traceEventType, "Unhandled EventType");
            }
        }

        protected static void WriteLogException(ILog libLogger, TraceEventType traceEventType, String message, Exception ex)
        {
            switch (traceEventType)
            {
                case TraceEventType.Critical:
                    libLogger.FatalException(message, ex);
                    break;
                case TraceEventType.Error:
                    libLogger.ErrorException(message, ex);
                    break;
                case TraceEventType.Warning:
                    libLogger.WarnException(message, ex);
                    break;
                case TraceEventType.Information:
                    libLogger.InfoException(message, ex);
                    break;
                case TraceEventType.Verbose:
                    libLogger.TraceException(message, ex);
                    break;
                case TraceEventType.Start:
                    libLogger.DebugException(message, ex);
                    break;
                case TraceEventType.Stop:
                    libLogger.DebugException(message, ex);
                    break;
                case TraceEventType.Suspend:
                    libLogger.DebugException(message, ex);
                    break;
                case TraceEventType.Resume:
                    libLogger.DebugException(message, ex);
                    break;
                case TraceEventType.Transfer:
                    libLogger.DebugException(message, ex);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(traceEventType), traceEventType, "Unhandled EventType");
            }
        }
    }

#if LIBLOG_PUBLIC
    public
#else
    internal
#endif
    class LibLogLogger : ILogger
    {
        private readonly ILog m_LibLogger;
        protected readonly Func<ILog, TraceEventType, bool> m_IsLogEventEnabledFunc;
        protected readonly Action<ILog, TraceEventType, String, Exception> m_WriteLogEventFunc;

        public LibLogLogger(ILog libLogger, Func<ILog, TraceEventType, bool> isLogEventEnabledFunc, Action<ILog, TraceEventType, String, Exception> writeLogEventFunc)
        {
            if (libLogger == null)
            {
                throw new ArgumentNullException(nameof(libLogger));
            }
            if (isLogEventEnabledFunc == null)
            {
                throw new ArgumentNullException(nameof(isLogEventEnabledFunc));
            }
            if (writeLogEventFunc == null)
            {
                throw new ArgumentNullException(nameof(writeLogEventFunc));
            }

            m_LibLogger = libLogger;
            m_IsLogEventEnabledFunc = isLogEventEnabledFunc;
            m_WriteLogEventFunc = writeLogEventFunc;
        }

        public bool WriteCore(TraceEventType traceEventType, Int32 eventId, Object state, Exception exception, Func<Object, Exception, String> formatter)
        {
            /// http://katanaproject.codeplex.com/SourceControl/latest#src/Microsoft.Owin/Logging/ILogger.cs
            /// To check IsEnabled call WriteCore with only TraceEventType and check the return value, no event will be written.
            if (state == null)
            {
                return m_IsLogEventEnabledFunc(m_LibLogger, traceEventType);
            }

            // no need to continue if event type isn't enabled
            if (!m_IsLogEventEnabledFunc(m_LibLogger, traceEventType))
            {
                return false;
            }

            m_WriteLogEventFunc(m_LibLogger, traceEventType, formatter(state, exception), exception);
            return true;
        }
    }

#if LIBLOG_PUBLIC
    public
#else
    internal
#endif
    static class LibLogFactoryExtensions
    {
        public static void UseLibLogging(this IAppBuilder app)
        {
            app.SetLoggerFactory(new LibLogLoggerFactory());
        }

        public static void UseLibLogging(this IAppBuilder app, Func<ILog, TraceEventType, bool> isLogEventEnabledFunc)
        {
            app.SetLoggerFactory(new LibLogLoggerFactory(isLogEventEnabledFunc));
        }

        public static void UseLibLogging(this IAppBuilder app, Action<ILog, TraceEventType, String, Exception> writeLogEventFunc)
        {
            app.SetLoggerFactory(new LibLogLoggerFactory(writeLogEventFunc));
        }

        public static void UseLibLogging(this IAppBuilder app, Func<ILog, TraceEventType, bool> isLogEventEnabledFunc, Action<ILog, TraceEventType, String, Exception> writeLogEventFunc)
        {
            app.SetLoggerFactory(new LibLogLoggerFactory(isLogEventEnabledFunc, writeLogEventFunc));
        }

        public static void UseLibLogging(this IAppBuilder app, Func<String, ILog> libLogConstructor, Func<ILog, TraceEventType, bool> isLogEventEnabledFunc, Action<ILog, TraceEventType, String, Exception> writeLogEventFunc)
        {
            app.SetLoggerFactory(new LibLogLoggerFactory(libLogConstructor, isLogEventEnabledFunc, writeLogEventFunc));
        }
    }
}
