using System;

namespace Tesco.Code
{
    public interface ITescoStopwatch
    {
        void Start();

        void Stop();

        void Reset();
        
        TimeSpan ElapsedTime { get; }
    }
}