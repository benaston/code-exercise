using System;

namespace Tesco.Code
{
    public class DisposableStopwatch : IDisposable
    {
        private readonly ITescoStopwatch _stopwatch;
        private readonly Action _continuation;

        public DisposableStopwatch(ITescoStopwatch stopwatch, Action continuation)
        {
            _stopwatch = stopwatch;
            _continuation = continuation;
            _stopwatch.Start();
        }

        public void Dispose()
        {
            _stopwatch.Stop();
            _continuation();
        }
    }
}