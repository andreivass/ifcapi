using System.ComponentModel.DataAnnotations;

namespace WebApi.Dtos
{
    /// <summary>
    /// Dto for JWT authorization login.
    /// </summary>
    public class LoginDto
    {
        [MaxLength(128)]
        [EmailAddress]
        [Required]
        public string? Email { get; set; }

        [MinLength(6)]
        [MaxLength(12)]
        [Required]
        public string? Password { get; set; }
    }
}
