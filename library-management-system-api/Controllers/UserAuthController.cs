using Abstractions;
using Entities.AuthModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace library_management_system_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAuthController : ControllerBase
    {
        private readonly IUserAuthManager _userAuthManager;

        public UserAuthController(IUserAuthManager userAuthManager)
        {
            _userAuthManager = userAuthManager;
        }


        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterModel registerModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userAuthManager.RegisterAsync(registerModel);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> GetTokenAsync([FromBody] TokenRequestModel tokenRequestModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userAuthManager.GetTokenAsync(tokenRequestModel);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("addrole")]
        public async Task<IActionResult> AddRoleAsync([FromBody] AddRoleModel addRoleModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userAuthManager.AddRoleAsync(addRoleModel);

            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            return Ok(addRoleModel);
        }
    }
}
