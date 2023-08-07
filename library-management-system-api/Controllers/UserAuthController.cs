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
    }
}
