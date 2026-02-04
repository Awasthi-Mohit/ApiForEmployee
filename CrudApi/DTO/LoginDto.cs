using System.ComponentModel.DataAnnotations;

namespace CrudApi.DTO
{
    public class LoginDto
    {
        [Required]

        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
