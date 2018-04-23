using System;

namespace SmartValley.Domain.Interfaces
{
    public interface IClock
    {
        DateTimeOffset UtcNow { get; }
    }
}