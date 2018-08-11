using System.Diagnostics;
using Castle.DynamicProxy;
using Olbrasoft.Travel.DataAccessLayer;

namespace Olbrasoft.Travel.ExpediaAffiliateNetwork.Import
{
    public class ImportInterceptor : IInterceptor
    {
        private readonly ILoggingImports _logger;

        public ImportInterceptor(ILoggingImports logger)
        {
            _logger = logger;
        }

        public void Intercept(IInvocation invocation)
        {
            //Before method execution
            var stopwatch = Stopwatch.StartNew();

            //Executing the actual method
            invocation.Proceed();

            //After method execution
            stopwatch.Stop();
            
            _logger.Log(
                $"DurationInterceptor: {invocation.MethodInvocationTarget.Name}" +
                $" executed in {stopwatch.Elapsed.TotalMilliseconds:0.000} milliseconds."
            );
        }
    }
}