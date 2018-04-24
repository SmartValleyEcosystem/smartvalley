using System;

namespace SmartValley.WebApi
{
    public class UtcClock : IClock
    {
        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    }
}