using Application_.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Application_.Interfaces
{
    public interface IProjectMemberService
    {
        Task<IActionResult> DeleteMemberAsync(DeleteProjectMemberDTO deleteProjectMemberDTO);
        Task<IActionResult> ListProjectMembersAsync(ListProjectMembersDTO listDTO);
        Task<IActionResult> SearchProjectMembersAsync(SearchProjectMembersRequestDTO searchRequest);

        Task<IActionResult> AssignMemberToProjectAsync(AssignProjectMemberDTO assignDTO);
    }
}