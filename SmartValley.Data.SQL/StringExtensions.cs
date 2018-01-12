using System;

namespace SmartValley.Data.SQL
{
    public static class StringExtensions
    {
        public static bool OrdinalEquals(this string a, string b)
        {
            return a.Equals(b, StringComparison.OrdinalIgnoreCase);
        }
    }
}