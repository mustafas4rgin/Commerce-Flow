using CommerceFlow.Services.Auth.Application.DTOs.User;
using CommerceFlow.Services.Auth.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommerceFlow.Services.Auth.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseApiController
    {
        private readonly IUserService _userService;
        public UserController(
            IUserService userService
        )
        {
            _userService = userService;
        }
        [HttpGet("users/all")]
        public async Task<IActionResult> GetAllUsersAsync(
            [FromQuery]UserFilterDTO dto,
            CancellationToken ct = default)
        {
            var result = await _userService.GetUsersAsync(dto, ct);

            return ToActionResult(result);
        }
        [HttpGet("users/{id:int}")]
        public async Task<IActionResult> GetUserByIdAsync([FromRoute]int id, CancellationToken ct = default)
        {
            var result = await _userService.GetUserByIdAsync(id, ct);

            return ToActionResult(result);
        }
        [HttpPost("users/add")]
        public async Task<IActionResult> AddUserAsync([FromBody]CreateUserDTO dto, CancellationToken ct = default)
        {
            var result = await _userService.CreateUserAsync(dto, ct);

            return ToActionResult(result);
        }
        [HttpPut("users/update/{id:int}")]
        public async Task<IActionResult> UpdateUserAsync([FromRoute]int id, [FromBody]UpdateUserDTO dto, CancellationToken ct = default)
        {
            var result = await _userService.UpdateUserByIdAsync(id, dto, ct);

            return ToActionResult(result);
        }
        [HttpDelete("users/delete/{id:int}")]
        public async Task<IActionResult> DeleteUserAsync([FromRoute]int id, CancellationToken ct = default)
        {
            var result = await _userService.DeleteUserByIdAsync(id, ct);

            return ToActionResult(result);
        }
    }
}
