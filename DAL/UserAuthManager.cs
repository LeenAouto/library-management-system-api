using Abstractions;
using AutoMapper;
using Entities;
using Entities.AuthModels;
using Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DAL
{
    public class UserAuthManager : IUserAuthManager
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWT _jwt;
        private readonly IMapper _mapper;
        private readonly ILogger<UserAuthManager> _logger;
        private readonly LibraryDbContext _context;
        public UserAuthManager(UserManager<AppUser> userManager, IOptions<JWT> jwt, RoleManager<IdentityRole> roleManager, IMapper mapper, ILogger<UserAuthManager> logger, LibraryDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwt = jwt.Value;
            _mapper = mapper;
            _logger = logger;
            _context = context;
        }

        public async Task<IEnumerable<AppUser>> GetAll()
        {
            try
            {
                return await _context.Users.ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
                throw;
            }
        }

        public async Task<AuthModel> RegisterAsync(RegisterModel model) //sign up
        {
            try
            {
                //Make sure that registered email is not already used by another user
                if (await _userManager.FindByEmailAsync(model.Email) != null)
                    return new AuthModel { Message = "Email is already registered" };

                //Make sure that registered username is not already used by another user
                if (await _userManager.FindByNameAsync(model.Username) != null)
                    return new AuthModel { Message = "Username is already registered" };

                var user = _mapper.Map<AppUser>(model);

                var result = await _userManager.CreateAsync(user, model.Password); //model.Password hashes the password before storing in database
                if (!result.Succeeded)
                {
                    var errors = string.Empty;
                    foreach (var error in result.Errors)
                    {
                        errors += $"{error.Description},";
                    }
                    return new AuthModel { Message = errors };
                }

                await _userManager.AddToRoleAsync(user, "User"); //User is the default role 

                var jwtSecurityToken = await CreateJwtToken(user);

                return new AuthModel
                {
                    Email = user.Email,
                    ExpiresOn = jwtSecurityToken.ValidTo,
                    IsAuthenticated = true,
                    Roles = new List<string> { "User" },
                    Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                    Username = user.UserName
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
                throw;
            }
        }

        public async Task<AuthModel> GetTokenAsync(TokenRequestModel model) //sign in
        {
            try
            {
                var authModel = new AuthModel();

                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    authModel.Message = "Email or password is incorrect";
                    return authModel;
                }

                var jwtSecurityToken = await CreateJwtToken(user);
                var roles = await _userManager.GetRolesAsync(user);

                authModel.IsAuthenticated = true;
                authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                authModel.Email = user.Email;
                authModel.Username = user.UserName;
                authModel.ExpiresOn = jwtSecurityToken.ValidTo;
                authModel.Roles = roles.ToList();

                return authModel;
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
                throw;
            }
        }

        public async Task<string> AddRoleAsync(AddRoleModel model) //adding a user to a role
        {
            try
            {
                var user = await _userManager.FindByIdAsync(model.UserId);

                if (user == null || !await _roleManager.RoleExistsAsync(model.RoleName))
                    return "Invalid UserId or RoleName";

                //check if the existing user is already assigned to the existing role
                if (await _userManager.IsInRoleAsync(user, model.RoleName))
                    return "User is already assigned to this role.";

                var result = await _userManager.AddToRoleAsync(user, model.RoleName);

                return result.Succeeded ? string.Empty : "Something went wrong";
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
                throw;
            }
        }

        public async Task<bool> UserExists(string userId)
        {
            try
            {
                if (await _userManager.FindByIdAsync(userId) == null)
                    return false;
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
                throw;
            }
        }

        private async Task<JwtSecurityToken> CreateJwtToken(AppUser user)
        {
            try
            {
                var userClaims = await _userManager.GetClaimsAsync(user);
                var roles = await _userManager.GetRolesAsync(user);
                var roleClaims = new List<Claim>();

                foreach (var role in roles)
                    roleClaims.Add(new Claim("roles", role));

                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("uid", user.Id)
                }
                .Union(userClaims)
                .Union(roleClaims);

                var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
                var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

                var jwtSecurityToken = new JwtSecurityToken(
                    issuer: _jwt.Issuer,
                    audience: _jwt.Audience,
                    claims: claims,
                    expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                    signingCredentials: signingCredentials);

                return jwtSecurityToken;
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
                throw;
            }
            
        }
    }
}
