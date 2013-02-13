using System;

namespace Tesco.Code
{
    public class AverageExecutionTimeDecorator<T> : IAuthorisationService where T : IAuthorisationService
    {
        private static object _requestCounterLock = new object();
        private static int _requestCounter;
        private readonly IAuthorisationService _decoratee;
        private readonly ITescoStopwatch _stopwatch;
        private readonly int _requestsPerLogEntry;
        private readonly ILogger _logger;
        private const string LogFormat = "AuthorisationService:Authorise  {0} averaged over {1} requests.";

        public AverageExecutionTimeDecorator(T decoratee, 
                                             ITescoStopwatch stopwatch, 
                                             int requestsPerLogEntry, 
                                             ILogger logger)
        {
            if(requestsPerLogEntry < 1)
            {
                throw new ArgumentOutOfRangeException("requestsPerLogEntry");
            }

            _decoratee = decoratee;
            _stopwatch = stopwatch;
            _requestsPerLogEntry = requestsPerLogEntry;
            _logger = logger;
        }

        public void Authorise(AuthorisationRequest request)
        {
            lock (_requestCounterLock)
            {
                _requestCounter++;

                if (_requestCounter < _requestsPerLogEntry)
                {
                    _decoratee.Authorise(request);

                    return;
                }
                
                using (new DisposableStopwatch(_stopwatch,
                                                () =>
                                                _logger.Log(string.Format(LogFormat,
                                                                            _stopwatch.ElapsedTime.ToString("ss"),
                                                                            _requestsPerLogEntry))))
                {
                    _decoratee.Authorise(request);
                }

                _stopwatch.Reset();
                _requestCounter = 0;
            }
        }
    }
}