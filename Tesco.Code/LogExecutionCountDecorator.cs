// ReSharper disable StaticFieldInGenericType
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tesco.Code
{
    public class LogExecutionCountDecorator<TDecoratee> 
        : IAuthorisationService where TDecoratee : IAuthorisationService
    {
        public const int MaximumSampleSize = 2000; //a random number
        public const string LogMessageFormat = "{0} AuthorisationService::Authorise {1} executions/second\n";

        protected static readonly IList<DateTime> SampleBuffer = new List<DateTime>();

        private readonly IAuthorisationService _decoratee;
        private readonly int _sampleSize;
        private readonly ILogger _logger;
        private readonly IClock _clock;

        public LogExecutionCountDecorator(TDecoratee decoratee,
            ILogger logger, IClock clock, int sampleSize = 100)
        {
            if (sampleSize < 1 || sampleSize > MaximumSampleSize)
            {
                throw new ArgumentOutOfRangeException("sampleSize");
            }

            _decoratee = decoratee;
            _logger = logger;
            _clock = clock;
            _sampleSize = sampleSize;
        }

        public void Authorise(AuthorisationRequest request)
        {
            RecordMetric();
            _decoratee.Authorise(request);
        }

        private void RecordMetric()
        {
            TimeSpan sampleTimeSpan;
            int bufferSize;

            lock (SampleBuffer)
            {
                SampleBuffer.Add(_clock.ApplicationNow);
                bufferSize = SampleBuffer.Count;

                if (bufferSize < _sampleSize)
                {
                    return;
                }

                sampleTimeSpan = SampleBuffer.Last() - SampleBuffer[0];
                SampleBuffer.Clear();
            }

            _logger.Log(string.Format(LogMessageFormat, _clock.ApplicationNow.ToString("o"), bufferSize/sampleTimeSpan.TotalSeconds));
        }
    }
}
// ReSharper restore StaticFieldInGenericType