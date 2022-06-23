using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApiBasic.Data;
using WebApiBasic.Dtos;
using WebApiBasic.Dtos.Account;
using WebApplication1.Models;

namespace WebApiBasic.Controllers
{
    [ApiController]
    [Route("Account")]
    [Authorize(Roles = "Admin")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly DbContext _context;
        private readonly IConfigurationSection _jwtSettings;


        public AccountController(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager, DBContext context
            , IConfiguration configuration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _jwtSettings = configuration.GetSection("JwtSettings");

        }



        [Route("/Register")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto model)
        {
            var user = new ApplicationUser()
            {
                Email = model.Email,
                UserName = model.UserName,
                IsActive = true
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var err in result.Errors)
                {
                    return BadRequest(err.Description);
                }
            }
            return StatusCode(201);
        }

        [Route("/Login")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            var checkPassword = _userManager.CheckPasswordAsync(user, model.Password);
            if (checkPassword.Result != false)
            {
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, true);
                var signingCredentials = GetSigningCredentials();
                var claims = GetClaims(user);
                var tokenOptions = GenerateTokenOptions(signingCredentials, await claims);

                var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                if (result.Succeeded)
                {
                    return Ok(new { Token = token });
                }
            }
            return BadRequest(" Password or Email not Valid");
        }
        [Route("/ForgetPassword")]
        [HttpPost]
        public async Task<IActionResult> ForgetPasswordAsync([FromBody] ForgetPasswordDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null)
            {
                return BadRequest("This User Not Exist");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var passwordResetLink = Url.Action("ResetPassword", "Account",
                       new { token = token }, Request.Scheme);

            return Ok(passwordResetLink);
        }
        [Route("/ResetPassword")]
        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null)
            {
                return BadRequest("This Email Not Have Account");
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (!result.Succeeded)
            {
                foreach (var err in result.Errors)
                {
                    return BadRequest(err.Description);
                }
            }
            return Ok("Password Reset Success");
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(_jwtSettings.GetSection("securityKey").Value);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }
        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var tokenOptions = new JwtSecurityToken(
            issuer: _jwtSettings.GetSection("validIssuer").Value,
            audience: _jwtSettings.GetSection("validAudience").Value,
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtSettings.GetSection("expiryInMinutes").Value)),
            signingCredentials: signingCredentials);
            return tokenOptions;
        }
        private async Task<List<Claim>> GetClaims(ApplicationUser user)
        {
            var claims = new List<Claim>()
{
new Claim(ClaimTypes.Name,user.Email)
};
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;

        }


        #region ExterNalLogin
        [Route("ExternalLogin")]
        [HttpPost]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account");


            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);


            return new ChallengeResult(provider, properties);
        }
        [Route("/Account/ExternalLoginCallback")]
        [HttpPost]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                return BadRequest(remoteError);
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info is null)
            {
                return BadRequest("Error loading external login Information");
            }
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            ApplicationUser user = null;
            if (email != null)
            {
                user = await _userManager.FindByEmailAsync(email);
            }
            var signinResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider,
                info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (signinResult.Succeeded)
            {
                return Ok("Success Is Login");
            }
            else
            {
                if (email != null)
                {
                    if (user is null)
                    {
                        user = new ApplicationUser
                        {
                            UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                        };
                        await _userManager.CreateAsync(user);
                    }
                    await _userManager.AddLoginAsync(user, info);
                    await _signInManager.SignInAsync(user, isPersistent: false);


                    return Ok("Success Is Login");

                }
            }
            return BadRequest("Please Contact support on Moh99@gmail.com");
        }
        #endregion
    }
}
