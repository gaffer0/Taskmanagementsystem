namespace Domain.Entities
{
    public class ProjectMember
    {
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;

        public Guid ProjectId { get; set; }
        public Project Project { get; set; } = null!;

        public string Role { get; set; } = "Member";
        public string? Team { get; set; }
    }
}
