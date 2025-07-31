using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application_.DTOs;
using Application_.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Taskmanagementsystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectMemberController : ControllerBase
    {
        private readonly IProjectMemberService _projectMemberService;

        public ProjectMemberController(IProjectMemberService projectMemberService)
        {
            _projectMemberService = projectMemberService;
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteMember([FromBody] DeleteProjectMemberDTO deleteDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return await _projectMemberService.DeleteMemberAsync(deleteDTO);
        }

        [HttpGet("list/{projectId}")]
        public async Task<IActionResult> ListProjectMembers(Guid projectId)
        {
            var listDTO = new ListProjectMembersDTO { ProjectId = projectId };
            return await _projectMemberService.ListProjectMembersAsync(listDTO);
        }

        [HttpPost("search")]
        public async Task<IActionResult> SearchProjectMembers([FromBody] SearchProjectMembersRequestDTO searchRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return await _projectMemberService.SearchProjectMembersAsync(searchRequest);
        }
    }
}
