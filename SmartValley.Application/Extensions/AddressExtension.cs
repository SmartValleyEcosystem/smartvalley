using Nethereum.Hex.HexConvertors.Extensions;

namespace SmartValley.Application.Extensions
{
    public static class AddressExtension
    {
        public static bool IsAddressEmpty(this string address) => string.IsNullOrWhiteSpace(address?.RemoveHexPrefix().Replace("0", ""));
    }
}
