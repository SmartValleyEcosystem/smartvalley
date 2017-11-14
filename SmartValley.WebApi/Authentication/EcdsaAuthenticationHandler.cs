using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
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
            if (!Request.Headers.TryGetValue(SvCustomCorsConstants.XEthereumAddress, out var ethereumAddess))
            {
                return Task.FromResult(AuthenticateResult.Fail($"Cannot read {SvCustomCorsConstants.XEthereumAddress} header."));
            }

            if (!Request.Headers.TryGetValue(SvCustomCorsConstants.XSignedText, out var signedText))
            {
                return Task.FromResult(AuthenticateResult.Fail($"Cannot read {SvCustomCorsConstants.XSignedText} header."));
            }

            if (!Request.Headers.TryGetValue(SvCustomCorsConstants.XSignature, out var signature))
            {
                return Task.FromResult(AuthenticateResult.Fail($"Cannot read {SvCustomCorsConstants.XSignature} header."));
            }

            try
            {
                var isValid = IsSignedMessageValid(ethereumAddess, signedText, signature);
                if (!isValid)
                {
                    return Task.FromResult(AuthenticateResult.Fail($"Signed message is invalid"));
                }
            }
            catch (Exception e)
            {
                return Task.FromResult(AuthenticateResult.Fail($"Signed message is invalid"));
            }

            var ticket = new AuthenticationTicket(new ClaimsPrincipal(new User(ethereumAddess, true)), EcdsaAuthenticationOptions.DefaultScheme);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }

        private bool IsSignedMessageValid(string ethAddress, string message, string signedMessage)
        {
            var publicKey = _ethereumMessageSigner.EncodeUTF8AndEcRecover(message, signedMessage);
            return publicKey.Equals(ethAddress, StringComparison.OrdinalIgnoreCase);
        }
    }
}