using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? FullName { get; set; }


        public ICollection<ProjectMember> ProjectMemberships { get; set; }
    }
}
