using Crud_Operation_Using_EF.Areas.Identity.Pages.Account;
using Crud_Operation_Using_EF.General;
using Crud_Operation_Using_EF.General.Email;
using Crud_Operation_Using_EF.Models;
using Crud_Operation_Using_EF.Models.ViewModels.Authentication.Login;
using Crud_Operation_Using_EF.Models.ViewModels.Authentication.SignUp;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using NuGet.Packaging.Signing;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Crud_Operation_Using_EF.General.SystemEnums;

namespace Crud_Operation_Using_EF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class AuthenticationController : ControllerBase
    {
        private readonly IEmailSender _emailSender;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _Configuration;
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthenticationController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager, IConfiguration configuration,IEmailSender emailSender)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._Configuration = configuration; 
            this._emailSender = emailSender; 
            this._signInManager = signInManager; 
        }
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterUser viewModel)
        { 
            if (ModelState.IsValid)
            { 
                var existingUser = await _userManager.FindByEmailAsync(viewModel.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "Email is already taken");
                    return BadRequest(ModelState);
                }

                existingUser = await _userManager.FindByNameAsync(viewModel.UserName);
                if (existingUser != null)
                {
                    ModelState.AddModelError("UserName", "Username is already taken");
                    return BadRequest(ModelState);
                }
                var newUser = new ApplicationUser { 
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,
                    UserName = viewModel.UserName,
                    Email = viewModel.Email 
                    
                };
                if (await _roleManager.RoleExistsAsync(viewModel.Role))
                {
                    var result = await _userManager.CreateAsync(newUser, viewModel.Password);

                    if (result.Succeeded)
                    {

                        await _userManager.AddToRoleAsync(newUser, viewModel.Role);

                        var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                        var tokenHtml = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

                        var link = EmailVerificationService.GenerateVerificationLink(newUser.Id, tokenHtml);

                        await _emailSender.SendEmailAsync(viewModel.Email, "Confirm your Mail", link);

                        return Ok("User created successfully...\nCheck you email for confirm registration");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        return BadRequest(ModelState);
                    }
                }
                else
                {
                    return BadRequest("This Role does not exist");

                }

                
            }

            return BadRequest(ModelState);
        }

       
        [HttpGet]
        public async Task<IActionResult> VerifyEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                // User not found
                return BadRequest("Invalid user ID");
            }

            // Verify the token
            var tokenBytes = WebEncoders.Base64UrlDecode(token);
            var tokenDecoded = Encoding.UTF8.GetString(tokenBytes);
            var isTokenValid = await _userManager.VerifyUserTokenAsync(user, "Default", "EmailConfirmation", tokenDecoded);

            if (!isTokenValid)
            {
                // Token is not valid
                return BadRequest("Invalid verification token");
            }

            // Mark the email as confirmed
            user.EmailConfirmed = true;
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                // Email confirmed successfully
                return Ok("Email confirmed successfully");
            }
            else
            {
                // Failed to update user
                return StatusCode(500, "Failed to update user");
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] LoginUserModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(viewModel.UserName);
                if (user == null)
                    return Unauthorized();

                var result = await _signInManager.CheckPasswordSignInAsync(user, viewModel.Password, false);
                if (!result.Succeeded)
                    return Unauthorized();

                var userRole = await _userManager.GetRolesAsync(user);

                var authClass = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, viewModel.UserName), 
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };

                foreach (var item in userRole)
                {
                    authClass.Add(new Claim(ClaimTypes.Role, item));
                }
                authClass.Add(new Claim(UsersDefinedClaimTypes.UserId.ToString(), user.Id));
                authClass.Add(new Claim(UsersDefinedClaimTypes.CompanyId.ToString(), viewModel.CompanyId.ToString()));
                

                var token = GenerateToken(authClass);

                return Ok(new {
                    token  = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                }); 
            }

            return BadRequest(ModelState);
        }

        private JwtSecurityToken GenerateToken(List<Claim> authClass)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Configuration["Jwt:Secret"]));


            var token = new JwtSecurityToken(
                issuer: _Configuration["Jwt:ValidIssuer"],
                audience: _Configuration["Jwt:ValidAudience"],
                expires: DateTime.UtcNow.AddDays(2),
                claims: authClass,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            );
            return token;
        }
    }
}
