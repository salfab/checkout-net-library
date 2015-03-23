using System;
namespace Checkout.CommonLibraries.Services.PerfTracker
{
    public interface IPerfTrackerService
    {
        void Dispose();
        string LogEventId { get; set; }
        void Run(Action codeBlock, string className = null, string methodName = null, string message = null, bool? shouldLog = null);
        T Run<T>(Func<T> codeBlock, string className = null, string methodName = null, string message = null, bool? shouldLog = null);
        void LogInformation(string className, string methodName, uint executionTime, string httpStatus, string message);
    }
}
