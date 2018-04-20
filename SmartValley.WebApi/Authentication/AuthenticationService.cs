using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using Nethereum.Signer;
using SmartValley.Application.Email;
using SmartValley.Domain.Core;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Exceptions;
using SmartValley.Domain.Interfaces;
using SmartValley.WebApi.Authentication.Requests;

namespace SmartValley.WebApi.Authentication
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class AuthenticationService : IAuthenticationService
    {
        private const double EmailCooldownMinutes = 1;
        private const string MemoryCacheEmailKey = "emailsent";

        private readonly IUserRepository _userRepository;
        private readonly EthereumMessageSigner _ethereumMessageSigner;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
        private readonly IClock _clock;
        private readonly MailService _mailService;
        private readonly MailTokenService _mailTokenService;
        private readonly IMemoryCache _memoryCache;

        public AuthenticationService(EthereumMessageSigner ethereumMessageSigner,
                                     IClock clock,
                                     IUserRepository userRepository,
                                     MailService mailService,
                                     MailTokenService mailTokenService,
                                     IMemoryCache memoryCache)
        {
            _ethereumMessageSigner = ethereumMessageSigner;
            _clock = clock;
            _userRepository = userRepository;
            _mailService = mailService;
            _mailTokenService = mailTokenService;
            _memoryCache = memoryCache;
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        }

        public async Task<Identity> AuthenticateAsync(AuthenticationRequest request)
        {
            var user = await _userRepository.GetByAddressAsync(request.Address);

            if (string.IsNullOrEmpty(user?.Email))
                throw new AppErrorException(ErrorCode.UserNotFound);

            if (!user.IsEmailConfirmed)
                throw new AppErrorException(ErrorCode.EmailNotConfirmed);

            if (!IsSignedMessageValid(request.Address, request.SignedText, request.Signature))
                throw new AppErrorException(ErrorCode.InvalidSignature);

            return await GenerateJwtAsync(user);
        }

        public async Task RegisterAsync(RegistrationRequest request)
        {
            if (!IsSignedMessageValid(request.Address, request.SignedText, request.Signature))
                throw new AppErrorException(ErrorCode.InvalidSignature);

            if (await DoesEmailExistAsync(request.Email))
                throw new AppErrorException(ErrorCode.EmailAlreadyExists);

            var user = await _userRepository.GetByAddressAsync(request.Address);
            if (user != null)
            {
                if (user.IsEmailConfirmed)
                    throw new AppErrorException(ErrorCode.AddressAlreadyExists);

                user.Email = request.Email;
                await _userRepository.UpdateWholeAsync(user);
            }
            else
            {
                user = new User
                       {
                           Address = request.Address,
                           Email = request.Email
                       };
                await _userRepository.AddAsync(user);
            }

            await _mailService.SendConfirmRegistrationAsync(user.Address, user.Email);
        }

        public async Task<Identity> RefreshAccessTokenAsync(Address address)
        {
            var user = await _userRepository.GetByAddressAsync(address);
            if (user == null)
                throw new AppErrorException(ErrorCode.UserNotFound);
            return await GenerateJwtAsync(user);
        }

        public bool ShouldRefreshToken(string encodedToken)
        {
            var token = _jwtSecurityTokenHandler.ReadJwtToken(encodedToken.Replace("Bearer ", ""));
            if (!token.Payload.ContainsKey("TokenIssueDate"))
                return true;

            var issueDate = (DateTime) token.Payload["TokenIssueDate"];

            return (_clock.UtcNow - issueDate).TotalMinutes >= AuthenticationOptions.LifetimeInMinutes;
        }

        public async Task ConfirmEmailAsync(string token)
        {
            var tokenValues = _mailTokenService.DecryptToken(token).Split(' ');
            var address = tokenValues[0];
            var email = tokenValues[1];
            var user = await _userRepository.GetByAddressAsync(address);
            if (user == null || !_mailTokenService.CheckEmailConfirmationToken(address, email, token))
                throw new AppErrorException(ErrorCode.IncorrectData);

            user.IsEmailConfirmed = true;
            user.Email = email;
            await _userRepository.UpdateWholeAsync(user);
        }

        public async Task ResendEmailAsync(Address address)
        {
            if (_memoryCache.TryGetValue(MemoryCacheEmailKey + address, out bool wasEmailSent))
                throw new AppErrorException(ErrorCode.EmailAlreadySent);

            var user = await _userRepository.GetByAddressAsync(address);

            await _mailService.SendConfirmRegistrationAsync(user.Address, user.Email);

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(1));

            _memoryCache.Set(MemoryCacheEmailKey + address, true, cacheEntryOptions);
        }

        public async Task ChangeEmailAsync(Address address, string email)
        {
            var user = await _userRepository.GetByAddressAsync(address);

            if (user == null)
                throw new AppErrorException(ErrorCode.UserNotFound);

            if (user.Email.Equals(email, StringComparison.OrdinalIgnoreCase))
                return;

            if (_memoryCache.TryGetValue(MemoryCacheEmailKey + address, out bool isEmailSent))
                throw new AppErrorException(ErrorCode.EmailAlreadySent);

            if (await DoesEmailExistAsync(email))
                throw new AppErrorException(ErrorCode.EmailAlreadyExists);

            await _mailService.SendUpdateEmailAsync(address, email);

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(EmailCooldownMinutes));

            _memoryCache.Set(MemoryCacheEmailKey + address, true, cacheEntryOptions);
        }

        public async Task<string> GetEmailBySignatureAsync(Address address, string signature, string signedText)
        {
            var user = await _userRepository.GetByAddressAsync(address);

            if (!IsSignedMessageValid(address, signedText, signature) || string.IsNullOrEmpty(user?.Email))
                return null;

            return user.Email;
        }

        private async Task<bool> DoesEmailExistAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            return user != null;
        }

        private static ClaimsIdentity CreateClaimsIdentity(User user, IReadOnlyCollection<Role> roles)
        {
            var claims = new List<Claim>
                         {
                             new Claim(ClaimsIdentity.DefaultNameClaimType, user.Address),
                             new Claim("UserId", user.Id.ToString())
                         };
            claims.AddRange(roles.Select(r => new Claim(ClaimsIdentity.DefaultRoleClaimType, r.Name)));

            return new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        }

        private async Task<Identity> GenerateJwtAsync(User user)
        {
            var now = _clock.UtcNow;
            var roles = await _userRepository.GetRolesByUserIdAsync(user.Id);
            var claimsIdentity = CreateClaimsIdentity(user, roles);

            var jwtSecurityToken = new JwtSecurityToken(
                                       AuthenticationOptions.Issuer,
                                       AuthenticationOptions.Audience,
                                       claimsIdentity.Claims,
                                       now.UtcDateTime,
                                       now.UtcDateTime.Add(TimeSpan.FromMinutes(AuthenticationOptions.LifetimeInMinutes)),
                                       new SigningCredentials(AuthenticationOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)) {Payload = {["TokenIssueDate"] = now}};

            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return new Identity(user.Id, user.Address, user.Email, true, token, roles.Select(r => r.Name).ToArray());
        }

        private bool IsSignedMessageValid(Address address, string signedText, string signature)
        {
            if (string.IsNullOrEmpty(signedText) || string.IsNullOrEmpty(signature))
                return false;

            var publicKey = (Address) _ethereumMessageSigner.EncodeUTF8AndEcRecover(signedText, signature);
            return publicKey == address;
        }
    }
}