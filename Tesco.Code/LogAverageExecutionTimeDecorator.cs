// ReSharper disable StaticFieldInGenericType
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tesco.Code
{
    /// <summary>
    /// Decorates implementations of the IAuthorisationService
    /// with logging of average execution time and number of executions.
    /// Use of generics is a little nasty, but it reduces type name length somewhat.
    /// 
    /// One problem with this implementation is that I am attempting to control access to 
    /// shared static variables manually, which is massively error-prone. Another problem is 
    /// that when the application shuts down then upto MaximumRequestsPerLogEntry timings may 
    /// never have a corresponding average logged.
    /// 
    /// Depending on the nature of the application within which the code sits, a different 
    /// "cache" location for the timing information might be preferred: for example the 
    /// ASP.NET data cache or an external tuple store.
    /// </summary>
    public class LogAverageExecutionTimeDecorator<TDecoratee> : IAuthorisationService where TDecoratee : IAuthorisationService
    {
        public const int MaximumRequestsPerLogEntry = 2000; //arbitrary guess based on random empirical timings
        public const string LogMessageFormat = "AuthorisationService::Authorise: {0}ms averaged over {1} requests.\n";

        private static readonly IList<double> Timings = new List<double>();

        private readonly IAuthorisationService _decoratee;
        private readonly ITescoStopwatch _stopwatch;
        private readonly int _requestsPerLogEntry;
        private readonly ILogger _logger;

        public LogAverageExecutionTimeDecorator(TDecoratee decoratee, 
                                                ITescoStopwatch stopwatch, 
                                                ILogger logger, int requestsPerLogEntry = 100)
        {
            if (requestsPerLogEntry < 1 || requestsPerLogEntry > MaximumRequestsPerLogEntry)
            {
                throw new ArgumentOutOfRangeException("requestsPerLogEntry");
            }

            _decoratee = decoratee;
            _stopwatch = stopwatch;
            _logger = logger;
            _requestsPerLogEntry = requestsPerLogEntry;
        }

        public void Authorise(AuthorisationRequest request)
        {
            using (new DisposableStopwatch(_stopwatch, RecordAndOrLog))
            {
                _decoratee.Authorise(request);
            }
        }

        /// <summary>
        /// Needed to ensure tests isolation with a static variable.
        /// Would prefer this was not on the real concrete implementation like this.
        /// </summary>
        public static void ResetRequestCounter()
        {
            lock (Timings)
            {
                Timings.Clear();
            }
        }

        /// <summary>
        /// Used as the continuation for the DisposableStopwatch.
        /// More idiomatic C# could be used if the "using" 
        /// stopwatch syntax was not being used.
        /// </summary>
        private void RecordAndOrLog()
        {
            double averageTiming;
            int requestCount;

            lock (Timings)
            {
                var elapsedMilliseconds = _stopwatch.ElapsedTime.TotalMilliseconds;
                Timings.Add(elapsedMilliseconds);
                requestCount = Timings.Count;

                if (requestCount < _requestsPerLogEntry && requestCount <= MaximumRequestsPerLogEntry)
                {
                    return;
                }

                averageTiming = Timings.Average();
                Timings.Clear();
            }

            //outside of the lock to minimise the length of the critical section
            //logging performance is of course very important (if it blocks)!
            _logger.Log(string.Format(LogMessageFormat, averageTiming, requestCount));
        }
    }
}
// ReSharper restore StaticFieldInGenericType