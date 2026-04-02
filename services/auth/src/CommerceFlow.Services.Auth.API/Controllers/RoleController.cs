using CommerceFlow.Services.Auth.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommerceFlow.Services.Auth.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : BaseApiController
    {
        private readonly IRoleService _roleService;
        public RoleController(
            IRoleService roleService
        )
        {
            _roleService = roleService;
        }

        [HttpGet("roles/all")]
        public async Task<IActionResult> GetAllRolesAsync([FromHeader]CancellationToken ct = default)
        {
            var roles = await _roleService.GetRolesAsync(ct);

            return Ok(roles);
        }
    }
}
