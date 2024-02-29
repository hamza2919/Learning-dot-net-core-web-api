using Crud_Operation_Using_EF.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;

namespace Crud_Operation_Using_EF.General.Email
{
    public class EmailVerificationService
    {
        public EmailVerificationService()
        {
                
        }
        public static string GenerateVerificationLink(string userId, string verificationToken)
        {
            // Construct the URL to your API endpoint for email verification
            var verificationUrl = "https://localhost:44342/api/Authentication?userId=" + userId + "&token=" + verificationToken;

            return verificationUrl;
        }
        //private readonly UserManager<ApplicationUser> _userManager;
        //private readonly IDataProtectionProvider _dataProtectionProvider;

        //public EmailVerificationService(UserManager<ApplicationUser> userManager, IDataProtectionProvider dataProtectionProvider)
        //{
        //    _userManager = userManager;
        //    _dataProtectionProvider = dataProtectionProvider;
        //}

        //public async Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user)
        //{
        //    var protector = _dataProtectionProvider.CreateProtector(typeof(EmailVerificationService).FullName);

        //    // Generate the token
        //    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        //    // Protect the token before storing it in the database
        //    return protector.Protect(token);
        //}

        //public async Task<bool> VerifyEmailConfirmationTokenAsync(ApplicationUser user, string token)
        //{
        //    var protector = _dataProtectionProvider.CreateProtector(typeof(EmailVerificationService).FullName);

        //    // Unprotect the token before verifying it
        //    var unprotectedToken = protector.Unprotect(token);

        //    // Verify the token
        //    var result = await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.EmailConfirmationTokenProvider, _userManager.Options.Tokens.EmailConfirmationTokenProvider, unprotectedToken);

        //    return result;
        //}

    }
}
