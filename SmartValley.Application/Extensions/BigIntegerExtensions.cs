using System;
using System.Numerics;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Web3;

namespace SmartValley.Application.Extensions
{
    public static class BigIntegerExtensions
    {
        public static Guid ToGuid(this BigInteger number)
        {
            var hexString = number.ToHex(false).Replace("0x", "");
            return new Guid(hexString.Length < 32 ? hexString.PadLeft(32, '0') : hexString);
        }

        public static double FromWei(this BigInteger number, int decimals)
        {
            return (double) Web3.Convert.FromWei(number, decimals);
        }
    }
}