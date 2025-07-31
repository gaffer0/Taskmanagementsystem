using Application_.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_.Interfaces
{
    public interface IProjectMemberService
    {
        Task<IActionResult> DeleteMemberAsync(DeleteProjectMemberDTO deleteProjectMemberDTO);
        Task<IActionResult> ListProjectMembersAsync(ListProjectMembersDTO listDTO);
        Task<IActionResult> SearchProjectMembersAsync(SearchProjectMembersRequestDTO searchRequest);
    }
}