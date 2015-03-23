using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkout.CommonLibraries.Services.PerfTracker
{
    public class PerfTrackerService : IPerfTrackerService, IDisposable
    {
        private const string PerformanceCallTrackerName="PerformanceCallTracker";
        /// <summary>
        /// Global setting of the PerfTracer for logging
        /// </summary>
        private bool _isLoggerEnabled;
        /// <summary>
        /// Relates the logs together
        /// </summary>
        public string LogEventId { get; set; }
        /// <summary>
        /// Nlog logger
        /// </summary>
        private readonly Logger _logger;

        private Stopwatch _executionTime;

        public PerfTrackerService(bool isLoggerEnabled)
        {
            _isLoggerEnabled = isLoggerEnabled;
            _executionTime = new Stopwatch();
        }


        /// <summary>
        ///  Executes the given Func<T> delegate and logs its performance.
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="codeBlock">code block to execute e.g. x=>x</param>
        /// <param name="className">class name to be logged</param>
        /// <param name="methodName">class name to be logged</param>
        /// <param name="message">message for the log</param>
        /// <param name="shouldLog">States if the loggin should be disabled or enabled for this call ignoring the global _isLoggerEnabled setting</param>
        /// <returns>returns the result object of the method call</returns>
        public T Run<T>(Func<T> codeBlock, string className = null, string methodName = null, string message = null, bool? shouldLog = null)
        {
            return CallFunc<T>(codeBlock, className, methodName, message, shouldLog);
        }

        /// <summary>
        /// Executes the given Action delegate and logs its performance.
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="codeBlock">code block to execute e.g. x=>x</param>
        /// <param name="className">class name to be logged</param>
        /// <param name="methodName">class name to be logged</param>
        /// <param name="message">message for the log</param>
        /// <param name="shouldLog">States if the loggin should be disabled or enabled for this call ignoring the global _isLoggerEnabled setting</param>
        public void Run(Action codeBlock, string className = null, string methodName = null, string message = null, bool? shouldLog = null)
        {
            CallAction(codeBlock, className, methodName, message, shouldLog);
        }

        private T CallFunc<T>(dynamic codeBlock, string className = null, string methodName = null, string message = null, bool? shouldLog = null)
        {
            var canLog = shouldLog ?? _isLoggerEnabled;

            if (!canLog)
            {
                return codeBlock.Invoke();
            }
            else
            {
                var response = default(T);

                _executionTime.Restart();

                try
                {
                    response = codeBlock.Invoke();
                    _executionTime.Stop();
                }
                catch (Exception)
                {
                    _executionTime.Stop();
                    throw;
                }
                finally
                {
                    LogInformation(className, methodName, Convert.ToUInt32(_executionTime.Elapsed.TotalMilliseconds), message);
                }

                return response;
            }
        }

        private void CallAction(dynamic codeBlock, string className = null, string methodName = null, string message = null, bool? shouldLog = null)
        {
            var canLog = shouldLog ?? _isLoggerEnabled;

            if (!canLog)
            {
                codeBlock.Invoke();
            }
            else
            {
                _executionTime.Restart();

                try
                {
                    codeBlock.Invoke();
                    _executionTime.Stop();
                }
                catch (Exception)
                {
                    _executionTime.Stop();
                    throw;
                }
                finally
                {
                    LogInformation(className, methodName, Convert.ToUInt32(_executionTime.Elapsed.TotalMilliseconds), message);
                }
            }
        }

        private void LogInformation(string className, string methodName, uint executionTime, string message)
        {
            var logEventInfo = new LogEventInfo(LogLevel.Info, PerformanceCallTrackerName, null);
            logEventInfo.Properties.Add("LOGEVENTID", LogEventId);
            logEventInfo.Properties.Add("CLASSNAME", className);
            logEventInfo.Properties.Add("METHODNAME", methodName);
            logEventInfo.Properties.Add("EXECUTIONTIME", executionTime);
            logEventInfo.Properties.Add("MESSAGE", message);

            LogManager.GetLogger(PerformanceCallTrackerName).Log(logEventInfo);
        }

        public void LogInformation(string className, string methodName, uint executionTime,string httpStatus, string message)
        {
            var logEventInfo = new LogEventInfo(LogLevel.Info, PerformanceCallTrackerName, null);
            logEventInfo.Properties.Add("CLASSNAME", className);
            logEventInfo.Properties.Add("METHODNAME", methodName);
            logEventInfo.Properties.Add("EXECUTIONTIME", executionTime);
            logEventInfo.Properties.Add("HTTPSTATUS", httpStatus);
            logEventInfo.Properties.Add("MESSAGE", message);

            LogManager.GetLogger(PerformanceCallTrackerName).Log(logEventInfo);
        }

        public void Dispose()
        {
            if (_executionTime != null && _executionTime.IsRunning)
            {
                _executionTime.Stop();
            }
        }
    }
}
