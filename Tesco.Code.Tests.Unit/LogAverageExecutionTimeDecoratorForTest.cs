namespace Tesco.Code.Tests.Unit
{
    public class LogAverageExecutionTimeDecoratorForTest<TDecoratee> : LogAverageExecutionTimeDecorator<TDecoratee>
        where TDecoratee : IAuthorisationService
    {
        public LogAverageExecutionTimeDecoratorForTest(TDecoratee decoratee, 
            ITescoStopwatch stopwatch, ILogger logger, IClock clock, int sampleSize = 100)
            : base(decoratee, stopwatch, logger, clock, sampleSize)
        {
        }

        /// <summary>
        /// Needed to ensure tests isolation with a static variable.
        /// </summary>
        public static void ResetRequestCounter()
        {
            lock (SampleBuffer)
            {
                SampleBuffer.Clear();
            }
        }
    }
}