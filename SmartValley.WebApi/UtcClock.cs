using System;
using SmartValley.Domain.Interfaces;

namespace SmartValley.WebApi
{
    public class UtcClock : IClock
    {
        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    }
}