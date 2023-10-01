using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SchedualingSystem.Interfaces;
using SchedualingSystem.JWTAuthentication;
using SchedualingSystem.Models.Authentication;
using SchedualingSystem.Models.Enums;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SchedualingSystem.Service
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWTAppSettings _jWTAppSettings;

        public IdentityService(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<JWTAppSettings> jWTAppSettings)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jWTAppSettings = jWTAppSettings.Value;
        }

        public async Task<LoginResponseViewModel> LoginAsync(LoginViewModel loginViewModel)
        {
            var user = await _userManager.FindByNameAsync(loginViewModel.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, loginViewModel.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = GetToken(authClaims);

                return new LoginResponseViewModel
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = token.ValidTo
                };
            }
            return new LoginResponseViewModel { };
        }

        public async Task<ResponseViewModel> RegisterAsync(RegisterViewModel registerViewModel)
        {
            var userExists = await _userManager.FindByNameAsync(registerViewModel.Username);
            if (userExists != null)
                return new ResponseViewModel { Status = "Error", Message = "User already exists!" };

            IdentityUser user = new()
            {
                Email = registerViewModel.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerViewModel.Username
            };
            var result = await _userManager.CreateAsync(user, registerViewModel.Password);
            if (!result.Succeeded)
                return new ResponseViewModel { Status = "Error", Message = "User creation failed! Please check user details and try again." };

            return new ResponseViewModel { Status = "Success", Message = "User created successfully!" };
        }

        public async Task<ResponseViewModel> RegisterAdminAsync(RegisterViewModel registerViewModel)
        {
            var userExists = await _userManager.FindByNameAsync(registerViewModel.Username);
            if (userExists != null)
                return new ResponseViewModel { Status = "Error", Message = "User already exists!" };

            IdentityUser user = new()
            {
                Email = registerViewModel.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerViewModel.Username
            };
            var result = await _userManager.CreateAsync(user, registerViewModel.Password);
            if (!result.Succeeded)
                return new ResponseViewModel { Status = "Error", Message = "User creation failed! Please check user details and try again." };

            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin.ToString()))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin.ToString()));
            if (!await _roleManager.RoleExistsAsync(UserRoles.User.ToString()))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User.ToString()));

            if (await _roleManager.RoleExistsAsync(UserRoles.Admin.ToString()))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.Admin.ToString());
            }
            if (await _roleManager.RoleExistsAsync(UserRoles.Admin.ToString()))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.User.ToString());
            }
            return new ResponseViewModel { Status = "Success", Message = "User created successfully!" };
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jWTAppSettings.Secret));

            var token = new JwtSecurityToken(
                issuer: _jWTAppSettings.ValidIssuer,
                audience: _jWTAppSettings.ValidAudience,
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
    }
}
