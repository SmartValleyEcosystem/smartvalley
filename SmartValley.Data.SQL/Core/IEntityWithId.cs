using System;

namespace SmartValley.Data.SQL.Core
{
    public interface IEntityWithId
    {
        Guid Id { get; set; }
    }
}
