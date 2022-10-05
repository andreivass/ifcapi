using System.ComponentModel.DataAnnotations;

namespace WebApi.Dtos
{
    /// <summary>
    /// Dto for JWT authorization user creation.
    /// </summary>
    public class RegisterDto : LoginDto
    {
        [MinLength(1)]
        [MaxLength(128)]
        public string? FirstName { get; set; }

        [MinLength(1)]
        [MaxLength(128)]
        public string? LastName { get; set; }
    }
}
