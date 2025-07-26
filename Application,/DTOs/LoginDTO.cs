using System.ComponentModel.DataAnnotations;

namespace Application_.DTOs
{
    public class LoginDTO
    {
        [Required]
        public string userName { get; set; }
        [Required]
        public string password { get; set; }
    }
}
