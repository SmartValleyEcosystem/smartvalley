using System;

namespace SmartValley.WebApi
{
    public interface IClock
    {
        DateTimeOffset UtcNow { get; }
    }
}