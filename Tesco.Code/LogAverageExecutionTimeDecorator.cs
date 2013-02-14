// ReSharper disable StaticFieldInGenericType
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tesco.Code
{
    public class LogAverageExecutionTimeDecorator<TDecoratee> 
        : IAuthorisationService where TDecoratee : IAuthorisationService
    {
        public const int MaximumSampleSize = 2000; //a random number
        public const string LogMessageFormat = "{0} AuthorisationService::Authorise {1}ms averaged over {2} executions\n";

        protected static readonly IList<double> SampleBuffer = new List<double>();

        private readonly IAuthorisationService _decoratee;
        private readonly ITescoStopwatch _stopwatch;
        private readonly int _sampleSize;
        private readonly ILogger _logger;
        private readonly IClock _clock;

        public LogAverageExecutionTimeDecorator(TDecoratee decoratee, ITescoStopwatch stopwatch, 
            ILogger logger, IClock clock, int sampleSize = 100)
        {
            if (sampleSize < 1 || sampleSize > MaximumSampleSize)
            {
                throw new ArgumentOutOfRangeException("sampleSize");
            }

            _decoratee = decoratee;
            _stopwatch = stopwatch;
            _logger = logger;
            _clock = clock;
            _sampleSize = sampleSize;
        }

        public void Authorise(AuthorisationRequest request)
        {
            using (new DisposableStopwatch(_stopwatch, RecordMetric))
            {
                _decoratee.Authorise(request);
            }
        }

        private void RecordMetric()
        {
            double averageTiming;
            int bufferSize;

            lock (SampleBuffer)
            {
                var elapsedMilliseconds = _stopwatch.ElapsedTime.TotalMilliseconds;
                SampleBuffer.Add(elapsedMilliseconds);
                bufferSize = SampleBuffer.Count;

                if (bufferSize < _sampleSize)
                {
                    return;
                }

                averageTiming = SampleBuffer.Average();
                SampleBuffer.Clear();
            }

            _logger.Log(string.Format(LogMessageFormat, _clock.ApplicationNow.ToString("o"), averageTiming, bufferSize));
        }
    }
}
// ReSharper restore StaticFieldInGenericType