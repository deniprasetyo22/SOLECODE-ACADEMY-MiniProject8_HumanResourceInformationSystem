using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniProject6.Application.DTOs.Account;
using MiniProject6.Application.Interfaces.IServices;

namespace MiniProject6.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authService.SignUpAsync(model);

            if (result.Status == "Error")
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginModel model)

        {

            if (!ModelState.IsValid)

                return BadRequest(ModelState);

            var result = await _authService.LoginAsync(model);

            if (result.Status == "Error")

                return BadRequest(result.Message);

            return Ok(result);

        }

        [HttpPost("create-role")]
        public async Task<IActionResult> CreateRoleAsync(string rolename)
        {
            var result = await _authService.CreateRoleAsync(rolename);
            return Ok(result);
        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignToRoleAsync(string userName, string rolename)
        {
            var result = await _authService.AssignToRoleAsync(userName, rolename);
            return Ok(result);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutModel model)
        {
            if (string.IsNullOrEmpty(model.Username))
            {
                return BadRequest(new ResponseModel { Status = "Error", Message = "Username is required!" });
            }

            var response = await _authService.LogoutAsync(model.Username);

            if (response.Status == "Success")
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
