using Concert.Application.Interface;
using Google.Authenticator;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Concert.Infrastructure.Service
{
    public class TwoFactorAuthService : IAuth
    {
        
        readonly IConfiguration _configuration;
        public TwoFactorAuthService( IConfiguration _configuration)
        {
            this._configuration = _configuration;
        }

        public bool KeyVerification(string email, string key)
        {
            var twoFactorSecretCode = _configuration["TwoFactorSecretCode"];
            string accountSecretKey = $"{twoFactorSecretCode}-{email}";
            var twoFactorAuthenticator = new TwoFactorAuthenticator();
            var result = twoFactorAuthenticator
                .ValidateTwoFactorPIN(accountSecretKey, key);
            return result;
        }

        public string TwoFactorAuthentication(string email)
        {
            var twoFactorAuthenticator = new TwoFactorAuthenticator();
            var twoFactorSecretCode = _configuration["TwoFactorSecretCode"];
            var accountSecretKey = $"{twoFactorSecretCode}-{email}";
            var setupCode = twoFactorAuthenticator.GenerateSetupCode("Concert API", email,
                Encoding.ASCII.GetBytes(accountSecretKey));
            return setupCode.ManualEntryKey;
        }

        
    }
}
