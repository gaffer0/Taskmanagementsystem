namespace Application_.DTOs
{
    public class AssignProjectMemberDTO
    {
        public Guid ProjectId { get; set; }
        public Guid MemberId { get; set; }
        public string Role { get; set; }
        public string Team { get; set; } // optional, if needed
    }
}
