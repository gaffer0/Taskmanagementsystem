namespace Application_.DTOs
{
    public class UserDeleteDTO
    {
        public Guid UserId { get; set; }
        public string Reason { get; set; } = string.Empty;
    }

    public class UserActivateDTO
    {
        public Guid UserId { get; set; }
        public bool IsActive { get; set; }
    }

    public class UserResponseDTO
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Role { get; set; } = null!;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
    }
} 