namespace Application_.DTOs
{
    public class CreateTaskDTO
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }
        public Guid AssignedUserId { get; set; }
        public DateTime? DueDate { get; set; }
        public string Priority { get; set; } = "Medium";
    }

    public class UpdateTaskDTO
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = "ToDo";
        public Guid AssignedUserId { get; set; }
        public DateTime? DueDate { get; set; }
        public string Priority { get; set; } = "Medium";
    }

    public class TaskResponseDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = null!;
        public string Priority { get; set; } = null!;
        public DateTime? DueDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; } = null!;
        public Guid AssignedUserId { get; set; }
        public string AssignedUserName { get; set; } = null!;
    }
} 