using System;

namespace Tesco.Code
{
    /// <summary>
    /// Helps test time-related beahvior.
    /// </summary>
    public class TescoClock : IClock
    {
        public DateTime ApplicationNow
        {
            get { return DateTime.UtcNow; }
        }
    }
}