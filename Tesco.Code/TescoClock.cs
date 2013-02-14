using System;

namespace Tesco.Code
{
    public class TescoClock : IClock
    {
        public DateTime ApplicationNow
        {
            get { return DateTime.UtcNow; }
        }
    }
}