using System;
using System.Diagnostics;

namespace Tesco.Code
{
    public class TescoStopwatch : ITescoStopwatch
    {
        readonly Stopwatch _stopwatch = new Stopwatch();

        public void Start()
        {
            _stopwatch.Start();
        }
        
        public void Stop()
        {
            _stopwatch.Stop();
        }
        
        public void Reset()
        {
            _stopwatch.Reset();
        }

        public TimeSpan ElapsedTime
        {
            get
            {
                return _stopwatch.Elapsed;
            }
        }
    }
}