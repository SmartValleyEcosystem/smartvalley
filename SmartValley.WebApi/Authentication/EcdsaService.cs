using System;
using Microsoft.Extensions.Primitives;
using Nethereum.Hex.HexConvertors;
using Nethereum.Signer;

namespace SmartValley.WebApi.Authentication
{
    public class EcdsaService
    {
        private readonly EthereumMessageSigner _ethereumMessageSigner;

        public EcdsaService()
        {
            _ethereumMessageSigner = new EthereumMessageSigner();
        }

        public bool IsSignedMessageValid(StringValues ethAddress, StringValues message)
        {
            var converter = new HexUTF8StringConvertor();
            var rawMesagge = converter.ConvertFromHex(message);
            var publicKey = _ethereumMessageSigner.EncodeUTF8AndEcRecover(rawMesagge, message);
            return publicKey.Equals(ethAddress, StringComparison.OrdinalIgnoreCase);
        }
    }
}