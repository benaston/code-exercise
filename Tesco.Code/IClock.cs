using System;

namespace Tesco.Code
{
    public interface IClock
    {
        DateTime ApplicationNow { get; }
    }
}