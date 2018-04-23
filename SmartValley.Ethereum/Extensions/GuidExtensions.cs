using System;
using System.Numerics;
using Nethereum.Hex.HexConvertors.Extensions;

namespace SmartValley.Ethereum.Extensions
{
    public static class GuidExtensions
    {
        public static BigInteger ToBigInteger(this Guid value)
            => value.ToString("N").HexToBigInteger(false);
    }
}