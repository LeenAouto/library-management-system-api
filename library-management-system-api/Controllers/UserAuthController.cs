using Abstractions;
using DAL;
using Entities.AuthModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace library_management_system_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAuthController : ControllerBase
    {
        private readonly IUserAuthManager _userAuthManager;
        private readonly ILogger<UserAuthManager> _logger;

        public UserAuthController(IUserAuthManager userAuthManager, ILogger<UserAuthManager> logger, LibraryDbContext context)
        {
            _userAuthManager = userAuthManager;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var users = await _userAuthManager.GetAll();
                if (!users.Any())
                    return NotFound($"No Users are found");

                return Ok(users);
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
                return BadRequest(e.Message);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterModel registerModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _userAuthManager.RegisterAsync(registerModel);

                if (!result.IsAuthenticated)
                    return BadRequest(result.Message);

                SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

                return Ok(result);
            }
            catch (Exception e){
                _logger.LogError(e.StackTrace);
                return BadRequest(e.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> GetTokenAsync([FromBody] TokenRequestModel tokenRequestModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _userAuthManager.GetTokenAsync(tokenRequestModel);

                if (!result.IsAuthenticated)
                    return BadRequest(result.Message);

                if (!string.IsNullOrEmpty(result.RefreshToken))
                    SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("addrole")]
        public async Task<IActionResult> AddRoleAsync([FromBody] AddRoleModel addRoleModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _userAuthManager.AddRoleAsync(addRoleModel);

                if (!string.IsNullOrEmpty(result))
                    return BadRequest(result);

                return Ok(addRoleModel);
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
                return BadRequest(e.Message);
            }
        }

        [HttpGet("refreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            try
            {
                var refreshToken = Request.Cookies["refreshToken"];

                var result = await _userAuthManager.RefreshTokenAsync(refreshToken);

                if (!result.IsAuthenticated)
                    return BadRequest(result);

                SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
                return BadRequest(e.Message);
            }
        }

        [HttpPost("revokeToken")]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeToken dto)
        {
            try
            {
                var token = dto.Token ?? Request.Cookies["refreshToken"];

                if (string.IsNullOrEmpty(token))
                    return BadRequest("Token is required");

                var result = await _userAuthManager.RevokeTokenAsync(token);

                if (!result)
                    return BadRequest("Token is Invalid");

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
                return BadRequest(e.Message);
            }
        }

        private void SetRefreshTokenInCookie(string refreshToken, DateTime expires)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = expires.ToLocalTime()
            };

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
    }
}
