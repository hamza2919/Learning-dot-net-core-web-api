using NuGet.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static Crud_Operation_Using_EF.General.SystemEnums;

namespace Crud_Operation_Using_EF.General.JWT
{
    public class JwtTokken
    {
        internal static object GetClaimValueFromTokkenDeodeSchemeByClaimType(string token, UsersDefinedClaimTypes ClaimType)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            if (jwtToken == null)
            {
                // Token cannot be read
                return null;
            }

            var userId = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimType.ToString())?.Value;
            // "sub" is commonly used for subject (user ID) in JWT tokens

            return userId;
        }
        internal static object GetClaimValueFromTokkenDeodeSchemeByClaimType(string tokken, string ClaimType)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(tokken) as JwtSecurityToken;

            if (jwtToken == null)
            {
                // Token cannot be read
                return null;
            }

            var userId = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimType)?.Value;
            // "sub" is commonly used for subject (user ID) in JWT tokens

            return userId;
        }
        internal static object GetClaimValueFromClaimPrincipalByClaimType(ClaimsPrincipal user, UsersDefinedClaimTypes ClaimType)
        {
            // Logic to extract the company ID from the user's claims or session
            // Implement according to your application's authentication mechanism
            // For demonstration purposes, let's assume the company ID is stored in a claim
            var claimValue = user.FindFirst(ClaimType.ToString())?.Value;
            return claimValue; // Return a default value or handle accordingly
        }
        internal static object GetClaimValueFromClaimPrincipalByClaimType(ClaimsPrincipal user, string ClaimType)
        {
            // Logic to extract the company ID from the user's claims or session
            // Implement according to your application's authentication mechanism
            // For demonstration purposes, let's assume the company ID is stored in a claim
            var claimValue = user.FindFirst(ClaimType.ToString())?.Value;
            return claimValue; // Return a default value or handle accordingly
        }
    }
}
