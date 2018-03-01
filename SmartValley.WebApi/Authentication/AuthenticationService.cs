﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using Nethereum.Signer;
using SmartValley.Application.Email;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Exceptions;
using SmartValley.Domain.Interfaces;
using SmartValley.WebApi.Authentication.Requests;

namespace SmartValley.WebApi.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly EthereumMessageSigner _ethereumMessageSigner;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
        private readonly IClock _clock;
        private readonly MailService _mailService;
        private readonly MailTokenService _mailTokenService;
        private readonly IMemoryCache _memoryCache;
        private double emailCooldownMinutes;
        private const string MemoryCacheEmailKey = "emailsended";

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

            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user != null)
                throw new AppErrorException(ErrorCode.EmailAlreadyExists);

            user = await _userRepository.GetByAddressAsync(request.Address);
            if (user != null)
                throw new AppErrorException(ErrorCode.AddressAlreadyExists);

            user = new User
                   {
                       Address = request.Address,
                       Email = request.Email
                   };

            await _userRepository.AddAsync(user);

            await _mailService.SendConfirmRegistrationAsync(user.Address, user.Email);
        }

        public async Task<Identity> RefreshAccessTokenAsync(string address)
        {
            var user = await _userRepository.GetByAddressAsync(address);
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

        public async Task ConfirmEmailAsync(string address, string token, string email)
        {
            var user = await _userRepository.GetByAddressAsync(address);
            if (user == null || user.IsEmailConfirmed || !_mailTokenService.CheckEmailConfirmationToken(address, email, token))
                throw new AppErrorException(ErrorCode.IncorrectData);

            user.IsEmailConfirmed = true;
            user.Email = email;
            await _userRepository.UpdateWholeAsync(user);
        }

        public async Task ResendEmailAsync(string address)
        {
            if (_memoryCache.TryGetValue(MemoryCacheEmailKey + address, out bool isEmailSended))
                throw new AppErrorException(ErrorCode.EmailAlreadySent);

            var user = await _userRepository.GetByAddressAsync(address);

            await _mailService.SendConfirmRegistrationAsync(user.Address, user.Email);

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(1));

            _memoryCache.Set(MemoryCacheEmailKey + address, true, cacheEntryOptions);
        }

        public async Task ChangeEmailAsync(string address, string email)
        {
            if (_memoryCache.TryGetValue(MemoryCacheEmailKey + address, out bool isEmailSent))
                throw new AppErrorException(ErrorCode.EmailAlreadySent);

            var user = await _userRepository.GetByEmailAsync(email);
            if (user != null)
                throw new AppErrorException(ErrorCode.EmailAlreadyExists);

            await _mailService.SendUpdateEmailAsync(address, email);

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(emailCooldownMinutes));

            _memoryCache.Set(MemoryCacheEmailKey + address, true, cacheEntryOptions);
        }

        public async Task<string> GetEmailBySignatureAsync(string address, string signature, string signedText)
        {
            var user = await _userRepository.GetByAddressAsync(address);

            if (!IsSignedMessageValid(address, signedText, signature) || string.IsNullOrEmpty(user?.Email))
                return null;

            return user.Email;
        }

        private ClaimsIdentity CreateClaimsIdentity(string address, IReadOnlyCollection<Role> roles)
        {
            var claims = new List<Claim>
                         {
                             new Claim(ClaimsIdentity.DefaultNameClaimType, address)
                         };
            claims.AddRange(roles.Select(r => new Claim(ClaimsIdentity.DefaultRoleClaimType, r.Name)));
            return new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        }

        private async Task<Identity> GenerateJwtAsync(User user)
        {
            var now = _clock.UtcNow;
            var roles = await _userRepository.GetRolesByUserIdAsync(user.Id);
            var claimsIdentity = CreateClaimsIdentity(user.Address, roles);

            var jwtSecurityToken = new JwtSecurityToken(
                AuthenticationOptions.Issuer,
                AuthenticationOptions.Audience,
                claimsIdentity.Claims,
                now.UtcDateTime,
                now.UtcDateTime.Add(TimeSpan.FromMinutes(AuthenticationOptions.LifetimeInMinutes)),
                new SigningCredentials(AuthenticationOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            jwtSecurityToken.Payload["TokenIssueDate"] = now;

            var jwt = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return new Identity(user.Address, true, jwt, roles.Select(r => r.Name).ToArray());
        }

        private bool IsSignedMessageValid(string address, string signedText, string signature)
        {
            if (string.IsNullOrEmpty(signedText) || string.IsNullOrEmpty(signature))
            {
                return false;
            }

            var publicKey = _ethereumMessageSigner.EncodeUTF8AndEcRecover(signedText, signature);
            return publicKey.Equals(address, StringComparison.OrdinalIgnoreCase);
        }
    }
}