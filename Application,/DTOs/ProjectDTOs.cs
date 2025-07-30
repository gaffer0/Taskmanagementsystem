namespace Application_.DTOs
{
    public class CreateProjectDTO
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public DateTime? DueDate { get; set; }
    }

    public class UpdateProjectDTO
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public DateTime? DueDate { get; set; }
    }

    public class ProjectResponseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public DateTime? DueDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public int TaskCount { get; set; }
        public int MemberCount { get; set; }
    }
} 