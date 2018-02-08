using Microsoft.AspNetCore.DataProtection;

namespace SmartValley.Application.Email
{
    public class MailTokenService
    {
        public const string MailTokenProtectorPurpose = "MailToken";
        private readonly IDataProtector _dataProtector;

        public MailTokenService(IDataProtectionProvider dataProtectionProvider)
        {
            _dataProtector = dataProtectionProvider.CreateProtector(MailTokenProtectorPurpose);
        }

        public string EncryptEmailConfirmationToken(string address, string email) =>
            _dataProtector.Protect(address + email);

        public bool CheckEmailConfirmationToken(string address, string email, string token) =>
            _dataProtector.Unprotect(token) == address + email;

        public string EncryptToken(string data) => _dataProtector.Protect(data);

        public string DecryptToken(string token) => _dataProtector.Unprotect(token);
    }
}