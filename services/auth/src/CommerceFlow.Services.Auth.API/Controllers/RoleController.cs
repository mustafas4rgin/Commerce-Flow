using CommerceFlow.Services.Auth.Application.DTOs.Role;
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
        [HttpDelete("roles/delete/{id:int}")]
        public async Task<IActionResult> DeleteRoleByIdAsync([FromRoute]int id, CancellationToken ct = default)
        {
            var result = await _roleService.DeleteRoleAsync(id, ct);

            return ToActionResult(result);
        }
        [HttpGet("roles/{id:int}")]
        public async Task<IActionResult> GetRoleByIdAsync([FromRoute]int id, CancellationToken ct = default)
        {
            var result = await _roleService.GetRoleByIdAsync(id, ct);

            return ToActionResult(result);
        }
        [HttpGet("roles/all")]
        public async Task<IActionResult> GetAllRolesAsync(CancellationToken ct = default)
        {
            var result = await _roleService.GetRolesAsync(ct);
            
            return ToActionResult(result);
        }
        [HttpPost("roles/add")]
        public async Task<IActionResult> AddRoleAsync([FromBody] CreateRoleDTO dto, CancellationToken ct = default)
        {
            var result = await _roleService.CreateRoleAsync(dto, ct);

            return ToActionResult(result);
        }
        [HttpPut("roles/update/{id:int}")]
        public async Task<IActionResult> UpdateRoleAsync([FromRoute]int id, [FromBody]UpdateRoleDTO dto, CancellationToken ct = default)
        {
            var result = await _roleService.UpdateRoleAsync(id, dto, ct);

            return ToActionResult(result);
        }
    }
}
