using Application_.DTOs;
using Application_.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost("assign")]
        [Authorize(Roles = "SuperAdmin,ProjectManager")]
        public async Task<IActionResult> AssignMember([FromBody] AssignProjectMemberDTO assignDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return await _projectMemberService.AssignMemberToProjectAsync(assignDTO);
        }
    }
}
