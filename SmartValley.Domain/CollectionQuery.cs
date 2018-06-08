using System;

namespace SmartValley.Domain
{
    public class CollectionQuery
    {
        public CollectionQuery(int offset, int count)
        {
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset), $"Should be greater or equal than zero, actual value: '{offset}'");

            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count), $"Should be greater than zero, actual value: '{count}'");

            Offset = offset;
            Count = count;
        }

        public int Offset { get; }
        
        public int Count { get; }
    }
}