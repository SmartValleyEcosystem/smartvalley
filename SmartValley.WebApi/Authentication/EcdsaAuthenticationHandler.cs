using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Nethereum.Signer;
using Nethereum.Hex.HexConvertors;
using SmartValley.WebApi.WebApi;

namespace SmartValley.WebApi.Authentication
{
    public class EcdsaAuthenticationHandler : AuthenticationHandler<EcdsaAuthenticationOptions>
    {
        private readonly EthereumMessageSigner _ethereumMessageSigner;

        public EcdsaAuthenticationHandler(IOptionsMonitor<EcdsaAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, EthereumMessageSigner signer)
            : base(options, logger, encoder, clock)
        {
            _ethereumMessageSigner = signer;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue(SvCustomCorsConstants.XNewEthereumAddress, out var ethAddress))
            {
                return Task.FromResult(AuthenticateResult.Fail($"Cannot read {SvCustomCorsConstants.XNewEthereumAddress} header."));
            }

            if (!Request.Headers.TryGetValue(SvCustomCorsConstants.XNewMessage, out var message))
            {
                return Task.FromResult(AuthenticateResult.Fail($"Cannot read {SvCustomCorsConstants.XNewMessage} header."));
            }

            if (!Request.Headers.TryGetValue(SvCustomCorsConstants.XNewSignedMessage, out var signedMessage))
            {
                return Task.FromResult(AuthenticateResult.Fail($"Cannot read {SvCustomCorsConstants.XNewSignedMessage} header."));
            }

            try
            {
                var isValid = IsSignedMessageValid(ethAddress, message, signedMessage);
                if (!isValid)
                {
                    return Task.FromResult(AuthenticateResult.Fail($"Signed message is invalid"));
                }
            }
            catch (Exception e)
            {
                return Task.FromResult(AuthenticateResult.Fail($"Signed message is invalid"));
            }

            var identities = new List<ClaimsIdentity> {new ClaimsIdentity(EcdsaAuthenticationOptions.DefaultScheme)};
            var ticket = new AuthenticationTicket(new ClaimsPrincipal(identities), EcdsaAuthenticationOptions.DefaultScheme);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }

        private bool IsSignedMessageValid(string ethAddress, string message, string signedMessage)
        {
            var converter = new HexUTF8StringConvertor();
            var rawMesagge = converter.ConvertFromHex(message);
            var publicKey = _ethereumMessageSigner.EncodeUTF8AndEcRecover(rawMesagge, signedMessage);
            return publicKey.Equals(ethAddress, StringComparison.OrdinalIgnoreCase);
        }
    }
}